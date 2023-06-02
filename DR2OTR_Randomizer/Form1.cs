

using DR2OTR_Randomizer.Resources;
using System.Data;
using System.Diagnostics;
using System.Net.WebSockets;

namespace DR2OTR_Randomizer;
/// <summary>
/// TODO:
///
/// Add a way to export changed stats to a xml file.
/// 
/// </summary>
public partial class F_ItemRandomiser : Form
{
    public List<ItemStatsData> VheicleStatData { get; set; }
    public List<ItemStatsData> NPCStatData { get; set; }
    public List<ItemStatsData> WorldStatsData { get; set; }
    public List<ItemStatsData> FireArmsStatData { get; set; }
    public List<ItemStatsData> ExplosiveStatData { get; set; }
    public List<ItemStatsData> FoodAndDamageData { get; set; }


    bool safeMode = true;

    AllItemStatData statData = new AllItemStatData();
    AllItemDataTable itemDataTable = new AllItemDataTable();

    //storeing these to skip stats
    //that cause items to crash the or break the item
    int[] unSafeLines = {
        17063, 17173, 17833, 17936, 18035, 18131, 18230, 18300, 18390, 18813, 18930, 19011,
        19092, 19152, 19260, 19355, 19436, 19524, 19722, 19872, 19971, 20064, 20152, 20259,
        20372, 20457, 20522, 20617, 20727, 20826, 77883, 77953, 78625, 78689, 78761, };


    LevelsLines levelLines = new LevelsLines();
    string path;
    DataTable allitemsTable = new DataTable();
    BindingSource allItemDataSource = new BindingSource();
    DataTable allUnStableitemsTable = new DataTable();
    BindingSource allUnstableItemSource = new BindingSource();
    public F_ItemRandomiser()
    {
        //gets all of the statdata stored inside the AllItemStatData
        //Most likely move over to a xml file for storing this
        VheicleStatData = (List<ItemStatsData>)statData.GetAllItemStatData()[0];
        NPCStatData = (List<ItemStatsData>)statData.GetAllItemStatData()[1];
        FireArmsStatData = (List<ItemStatsData>)statData.GetAllItemStatData()[2];
        WorldStatsData = (List<ItemStatsData>)statData.GetAllItemStatData()[3];
        ExplosiveStatData = (List<ItemStatsData>)statData.GetAllItemStatData()[4];
        FoodAndDamageData = (List<ItemStatsData>)statData.GetAllItemStatData()[5];

        allitemsTable = itemDataTable.SetAllItemData();
        allUnStableitemsTable = itemDataTable.SetAllItemData();

        InitializeComponent();
        //use this to catch if the Allitems or npcmodels file is missing
        try
        {
            File.Exists($"{Application.StartupPath}\\Resources\\AllItemData.xml");
            File.Exists($"{Application.StartupPath}\\Resources\\AllNPCModels.txt");
        }
        catch (FileNotFoundException e)
        {
            MessageBox.Show
            ($"{e.FileName} \nIs missing or as been renamed. The program cannot run without this file"
            , "Warning");
            Process.GetCurrentProcess().Kill();
        }
        //Set the datatable for the item stats data grid
        dgv_ItemStatsTable.DataSource = VheicleStatData;
        //sets the binding source and data table for both of the 
        //all item data grid views
        dgv_AllItems.DataSource = allitemsTable;
        Set_dgv_AllItems_Format(dgv_AllItems);
        allItemDataSource.DataSource = allitemsTable;

        dgv_US_Items.DataSource = allUnStableitemsTable;
        Set_dgv_AllItems_Format(dgv_US_Items);
        allUnstableItemSource.DataSource = allUnStableitemsTable;
    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }
    private void Set_dgv_AllItems_Format(DataGridView dataGrid)
    {
        //Sets the formating for the all item data grid
        dataGrid.Columns[0].Width = 75;
        dataGrid.Columns[0].HeaderText = "Enabled";
        dataGrid.Columns[1].ReadOnly = true;
        dataGrid.Columns[1].HeaderText = "Item Name";
        dataGrid.Columns[1].Width = 250;
        dataGrid.Columns[2].Visible = false;
        dataGrid.Columns[2].ReadOnly = true;
    }
    private void tc_TabWindowsTabSelect(object sender, EventArgs e)
    {
        //changes the current selected cell to not cause issues
        //with check state and it not updating the check state
        dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
    }
    private void b_DeselectAll_Click(object sender, EventArgs e)
    {
        ///Get the current filter and store it to later reapply it
        ///Remove any filters form the table
        ///Then gose through the allitems datatable rows
        ///And sets each cell[0] that are bools to false
        ///Then reapplys the filter storted from the start
        string storedFilter = allItemDataSource.Filter;
        allItemDataSource.RemoveFilter();
        for (int i = 0; i < allitemsTable.Rows.Count; i++)
        {
            dgv_AllItems.Rows[i].Cells[0].Value = false;
        }
        allItemDataSource.Filter = storedFilter;
    }
    private void bt_IS_UnstableUncheck_Click(object sender, EventArgs e)
    {
        string storedFillter = allUnstableItemSource.Filter;
        allUnstableItemSource.RemoveFilter();
        for (int i = 0; i < allUnstableItemSource.Count; i++)
        {
            dgv_US_Items.Rows[i].Cells[0].Value = false;
        }
        allUnstableItemSource.Filter = storedFillter;
    }
    private void b_ToggleAll_Click(object sender, EventArgs e)
    {
        //Will toggle check state  on all items in the 
        //all items data grid view
        ToogleAllItemsCheckState(dgv_AllItems);
    }
    private void bt_IS_UnstableToggle_Click(object sender, EventArgs e)
    {
        //Toggle the check state on the unstable items data grid
        ToogleAllItemsCheckState(dgv_US_Items);
    }
    private void tsm_open_Click(object sender, EventArgs e)
    {
        //gets the path of the datafile folder
        fbd_DataFileFolder.ShowDialog(this);
        path = fbd_DataFileFolder.SelectedPath;
    }
    private void tb_ItemsSearch_TextChanged(object sender, EventArgs e)
    {
        //Searches all items in the item data source
        if (tb_ItemsSearch.Text.StartsWith("#"))
        {
            //When the # is added at the start of text box it will
            //show the tags and when enabled as been tpyed in after
            //it it will show only the currently enabled items
            dgv_AllItems.Columns[2].Visible = true;
            dgv_AllItems.Columns[1].Width = 150;
            if (tb_ItemsSearch.Text.ToLower() == "#enabled")
            {
                allItemDataSource.Filter = "ItemState";
            }
        }
        else
        {
            //hides the tag column when the # is removed and will
            //only search for item name
            dgv_AllItems.Columns[1].Width = 250;
            dgv_AllItems.Columns[2].Visible = false;
            allItemDataSource.Filter = $"ItemName LIKE '*{tb_ItemsSearch.Text}*'";
        }
    }
    private void tb_US_ItemSearch_TextChanged(object sender, EventArgs e)
    {
        //Adding a search character for the unstable items to search though the 
        //tags due to there being no tags button/tabs
        if (tb_US_SearchBox.Text.Trim().StartsWith('#'))
        {
            dgv_US_Items.Columns[2].ReadOnly = true;
            dgv_US_Items.Columns[1].Width = 150;
            allUnstableItemSource.Filter = $"ItemTag LIKE '*{tb_US_SearchBox.Text.Split('#')[1]}*'";
            if (tb_US_SearchBox.Text.ToLower() == "#enabled")
            {
                allUnstableItemSource.Filter = $"ItemState = true";
            }
        }
        //search the item name and hides the item tag collum
        else
        {
            dgv_US_Items.Columns[1].Width = 250;
            dgv_US_Items.Columns[2].Visible = false;
            allUnstableItemSource.Filter = $"ItemName LIKE '*{tb_US_SearchBox.Text}*'";
        }
    }
    private void safeModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //Used to eanble/disable safe mode and change the text to show if it is enabled
        var result = MessageBox.Show("Disabling safe mode will randomize more areas but with a much higher chance of crashing", "Warning", MessageBoxButtons.YesNo);
        if (result == DialogResult.No)
        {
            safeMode = true;
            l_SafeMode_Text.Text = "Safe Mode Enabled";
            l_SafeMode_Text.ForeColor = Color.Green;
            safeModeToolStripMenuItem.Text = "Safe Mode Enabled";
        }
        if (result == DialogResult.Yes)
        {
            safeMode = false;
            safeModeToolStripMenuItem.Text = "Safe Mode Disabled";
            l_SafeMode_Text.Text = "Safe Mode Disabled";
            l_SafeMode_Text.ForeColor = Color.Red;
        }
    }
    private void bt_NPC_Model_Randomizer_Click(object sender, EventArgs e)
    {
        //dose a check to see if items.txt can be found if cant it will return
        if (!File.Exists($"{path}\\items.txt")) { MessageBox.Show("Could not find items.txt", "Warning"); return; }
        //Adds each line from the items.txt file to an array
        string[] itemFile = File.ReadAllLines($"{path}\\items.txt");
        var confirmResults = MessageBox.Show
            ("This will change all NPC models apart from TK and Snow Flake." +
            "\n\n\tAre you sure you want to change NPC models?", "Warning", MessageBoxButtons.YesNo);
        if (confirmResults == DialogResult.Yes)
        {
            RandomizeNPCModels(itemFile);
            File.WriteAllLines($"{path}\\items.txt", itemFile);
        }
        else
        {
            return;
        }
    }
    private void tc_Items_SelectedTab(object sender, EventArgs e)
    {
        string tabPageTag = "";
        if (tc_Items.SelectedTab.Name != "tp_AllItems")
        {
            tabPageTag = tc_Items.SelectedTab.Tag.ToString();
        }
        //Uses a tab control to fillter items tag with each tab page
        //having there respective item tag stored inside of tab page tag
        switch (tc_Items.SelectedTab.Name)
        {
            case "tp_AllItems":
                allItemDataSource.Filter = "";
                break;
            case "tp_BasicCombo":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_BasicFood":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_BasicLarge":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_BasicSmall":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Bugged":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Clothing":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_CombinedFireArmsSpray":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_CombinedFoodSpoiled":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_CombinedThowingMelee":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_ComboFireArmSpray":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_DLC":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Explosive":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_KeyItems":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Magazines":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Mannequin":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_PushPlaced":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Special":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;
            case "tp_Vehicles":
                allItemDataSource.Filter = $"ItemTag = '{tabPageTag}'";
                break;

        }
    }
    private void tc_itemStats_SelectedTab(object sender, EventArgs e)
    {
        //when changing tab the selected cell will move over 1 so there is no issues with tolggle them
        switch (tc_itemStats.SelectedTab.Name)
        {
            case "tp_IS_VehicleStats":
                dgv_ItemStatsTable.DataSource = VheicleStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_IS_NPC":
                dgv_ItemStatsTable.DataSource = NPCStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.CurrentRow.Cells[1];
                break;
            case "tp_IS_FireArms":
                dgv_ItemStatsTable.DataSource = FireArmsStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_IS_WorldStats":
                dgv_ItemStatsTable.DataSource = WorldStatsData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_IS_ExplosivesSpray":
                dgv_ItemStatsTable.DataSource = ExplosiveStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_IS_FoodDamage":
                dgv_ItemStatsTable.DataSource = FoodAndDamageData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
        }
        //If the unstable tab is selected send the item stats data
        //grid view to he back to hide it and show the unstable items
        if (tc_itemStats.SelectedTab.Name == "tp_IS_UnstableStats")
        { dgv_ItemStatsTable.SendToBack(); }
        else { dgv_ItemStatsTable.BringToFront(); }
    }
    private void dgv_ItemStatsTable_ColumnHeaderClicked(object sender, DataGridViewCellMouseEventArgs e)
    {
        //If the header is clicked on the item stat data grid this
        //will toggle the check state on the current item stats
        foreach (DataGridViewRow row in dgv_ItemStatsTable.Rows)
        {
            var objectToBool = Convert.ToBoolean(row.Cells[0].Value);
            objectToBool = !objectToBool;
            row.Cells[0].Value = objectToBool;
        }
    }
    private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        //Detects if a non numeic number as been pressed then stops
        //it from beeing added to the field
        e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
        if (dgv_ItemStatsTable.CurrentCell.ColumnIndex == 3 ||
            dgv_ItemStatsTable.CurrentCell.ColumnIndex == 4)
        {
            TextBox tb = (TextBox)e.Control;
            if (tb != null)
            {
                tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
            }
        }
    }
    private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs anError)
    {
        if (anError.Context == DataGridViewDataErrorContexts.Commit)
        {
            //if the cell is left blank change it back to 0
            dgv_ItemStatsTable.CurrentCell.Value = 0;
        }
    }
    private void Column1_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }
    private void b_Randomise_Click(object sender, EventArgs e)
    {

        //make a string list to store all the items in each of the check list boxes
        List<string> allItems = new List<string>();

        GetItemsToRandomize(allItems);
        //Stop here if no items have been checked and added to the list
        if (allItems.Count <= 0) { MessageBox.Show("No items have been selected", "Warning"); return; }
        //Changes the needed lines inside the item txt file
        //to prevent crashing and soft locking
        SoftLockAndCrashPrevent(b_Randomise.Text, allItems);
        RandomizeGameItems(allItems);
    }
    private void bt_ItenStatsSet_Click(object sender, EventArgs e)
    {
        //Checks to see if the item.txt\mission file can be found if not it will return with a warning message
        if (!File.Exists($"{path}\\items.txt") || !File.Exists($"{path}\\missions.txt"))
        { MessageBox.Show("Issues finding item.txt and mission.txt", "Warning"); return; }
        string[] itemFile = File.ReadAllLines($"{path}\\items.txt");
        string[] missionFile = File.ReadAllLines($"{path}\\missions.txt");

        RandomizeUnstableStats(itemFile, missionFile);
        RandomizeItemStats(itemFile);

        //passesing a null overide for this button as a list is not needed
        SoftLockAndCrashPrevent(bt_ItenStatsSet.Text, null);
        MessageBox.Show("Item stats have successfully been randomized", "Success");
    }
    private void dgv_itemStatTabel_CellSelected(object sender, DataGridViewCellMouseEventArgs e)
    {
        for (int i = 0; i < dgv_ItemStatsTable.SelectedCells.Count; i++)
        {
            if (dgv_ItemStatsTable.SelectedCells[i].OwningColumn.DataPropertyName == "StatState")
            {
                var checkTobool = Convert.ToBoolean(dgv_ItemStatsTable.SelectedCells[i].Value);
                checkTobool = !checkTobool;
                dgv_ItemStatsTable.SelectedCells[i].Value = checkTobool;
                //Selects another cell so there is no issues when selecting the header
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.CurrentRow.Cells[1];
            }
        }
    }
    private void tsm_Unpacker_Click(object sender, EventArgs e)
    {
        var result = MessageBox.Show("This will open a webpage for downloading the Gibbed dead rising 2 .big unpack/repack tool. \n\nDo you want to continue?", "Warning", MessageBoxButtons.YesNo);

        if (result == DialogResult.Yes)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://www.mediafire.com/file/dfy5825qhufmpr6/Gibbed.DeadRising2.Tools_%2528rev11%2529.rar/file",
                UseShellExecute = true,
            });
        }
        else
        {
            return;
        }
    }
    private void tsm_Credits_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Devleped by Fairy with a Pan", "Credits");
    }
    private void tsm_Quit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
    private void SoftLockAndCrashPrevent(string buttonClicked, List<string>? Allitems)
    {
        List<string> missionFile = new List<string>();
        //checks to see if the mission txt is inside the path folder
        if (!File.Exists($"{path}\\missions.txt")) { MessageBox.Show("No mission.txt file found", "Warning"); return; }
        missionFile.AddRange(File.ReadAllLines($"{path}\\missions.txt"));
        bool showOnce = false;
        //This will stop overtime softlocking if the player reloads while in overtime
        if (buttonClicked == b_Randomise.Text)
        {
            for (int i = 0; i < allitemsTable.Rows.Count; i++)
            {
                bool checkState = Convert.ToBoolean(allitemsTable.Rows[i].ItemArray[0]);
                //Checks if any Key items have been selected and will
                //clear the line that requirse the player to collect
                //all the items for TK in overtime. this will mean the
                //user can go straight to the arnea once starting overtime
                if (checkState && allitemsTable.Rows[i].ItemArray[2].ToString() == "KeyItems")
                {
                    missionFile[50046] = "";
                }
                if (checkState && allitemsTable.Rows[i].ItemArray[2].ToString() == "Vehicles" && !showOnce)
                {
                    MessageBox.Show("Having too many vehicles in one area can cause the game to be unstable or crash", "Warning");
                    //Stops the mesagebox from showing more then once
                    showOnce = true;
                }
            }
        }
        //Change these so player is not instantly gunned down when getting to case 8-3 fight inside the lab
        if (buttonClicked == bt_ItenStatsSet.Text)
        {
            missionFile[30323] = "\t\t\t\t\tWithProp = \"BobsToy\"";
            missionFile[30332] = "\t\t\t\t\tWithProp = \"BobsToy\"";
        }
        File.WriteAllLines($"{path}\\missions.txt", missionFile);
    }
    private void ToogleAllItemsCheckState(DataGridView dataGrid)
    {
        //stores the applied filter
        string storeFilter = allItemDataSource.Filter;
        for (int i = 0; i < dataGrid.Rows.Count; i++)
        {
            //Gets the each rows ItemState and converts them from check
            //to a bool for easier use
            //the data grid cells are changed to force the table to
            //update changes to the enabled(ItemState) colloum
            bool checkToBool = Convert.ToBoolean(dataGrid.Rows[i].Cells[0].Value);
            dataGrid.CurrentCell = dataGrid.Rows[0].Cells[1];
            if (tb_ItemsSearch.Text.ToLower() == "#enabled")
            {
                //Checks if filtering only for enabled items only
                //then will do the same as the uncheck all button
                allItemDataSource.Filter = "";
                if (checkToBool)
                {
                    dataGrid.Rows[i].Cells[0].Value = false;
                }
            }
            else
            {
                checkToBool = !checkToBool;
                dataGrid.Rows[i].Cells[0].Value = checkToBool;
            }
            dataGrid.CurrentCell = null;

        }
        //reaplly any filters if removed
        allItemDataSource.Filter = storeFilter;
    }
    private void GetItemsToRandomize(List<string> allItems)
    {
        //use the allitemtable BindingSource to go through
        //each row and ignore any applied fittlers
        for (int i = 0; i < allitemsTable.Rows.Count; i++)
        {
            //Gets the check box from the current row 
            //And store it as a bool to check its value
            bool checkState = Convert.ToBoolean(allitemsTable.Rows[i].ItemArray[0]);
            if (checkState == true)
            {
                allItems.Add(allitemsTable.Rows[i].ItemArray[1].ToString());
            }
        }
    }
    private void RandomizeGameItems(List<string> allItems)
    {

        //gets the dictionary stored in the LevelLines class
        var levels = LevelsLines.levels;
        Random rand = new Random();
        //goese though each of the levels in side of the dictionary
        foreach (var level in levels)
        {
            //checks to see that the selected path has all the requied files
            if (!File.Exists($"{path}\\{level.Value}"))
            {
                //returns if it cant find any
                MessageBox.Show($"Could not find {level.Value} please check your datafile folder", "Warning");
                continue;
            }
            //gets the current level file in the dictionary with the level.Value is the same as the files name
            string[] levelFile = File.ReadAllLines($"{path}\\{level.Value}");
            //adds all the lines inside of the current selected level to an array
            foreach (int line in level.Key)
            {

                //skips palisades if in safemode due to crashing
                if (safeMode && level.Value == "palisades.txt") { continue; }
                int item = rand.Next(allItems.Count);

                levelFile[line - 1] = levelFile[line - 1].Split('=')[0] + $"= {allItems[item]}";
            }
            //Writes all the lines inside of the levelfile array to the levels txt file
            File.WriteAllLines($"{path}\\{level.Value}", levelFile);

        }
        MessageBox.Show
        ("After case 7-1 once the gas as released the platinum and silver strip becomes unstable and can crash." +
        " Its recommend to save before going to these places (the fortune park save point is safe and recommend).",
        "Warning");
        MessageBox.Show("All levels successfully randomised with selected items", "Success");
    }
    private void RandomizeItemStats(string[] itemFile)
    {
        Random rand = new Random();
        //adds all of the item stat list to an array for
        //the foreach loop to go though each stat and get there
        //valuse and set them to the item.txt file
        object[] itemStatLists =
        {
            VheicleStatData,
            NPCStatData,
            FireArmsStatData,
            WorldStatsData,
            ExplosiveStatData,
            FoodAndDamageData,
        };
        foreach (List<ItemStatsData> itemStats in itemStatLists)
        {
            for (int i = 0; i < itemStats.Count; i++)
            {
                if (itemStats[i].StatState == false) continue;
                int[] minMax = { itemStats[i].StatMin, itemStats[i].StatMax };
                Array.Sort(minMax);
                string currentStat = itemStats[i].StatInGameName;
                for (int i1 = 0; i1 < itemFile.Length; i1++)
                {
                    if (itemFile[i1].Contains(currentStat))
                    {
                        if (unSafeLines.Contains(i1 + 1) && safeMode) { continue; }
                        int randStatNumb = rand.Next(minMax[0], minMax[1]);
                        itemFile[i1] =
                            itemFile[i1].Split('=')[0] + $"= \"{randStatNumb}\"";
                    }
                }
            }
        }
        File.WriteAllLines($"{path}\\items.txt", itemFile);
    }
    private void RandomizeNPCModels(string[] itemFile)
    {
        //store these lines to skip them so it dose not randomize 
        //the models as these can crash or do not work proply
        int[] ignoreNPC = { 28562, 28579, 28888, 28919, 28943, 28966, 28989, 29013,
            29135, 29165, 30401, 94601, 94647, 96381, 96439, 96492 };
        List<string> npcModels = new List<string>();
        Random rand = new Random();

        npcModels.AddRange(File.ReadAllLines($"{Application.StartupPath}\\Resources\\AllNPCModels.txt"));

        for (int i = 0; i < itemFile.Length; i++)
        {
            if (itemFile[i].Contains($"\tAssetFilename = \"data/models/npcs/"))
            {
                int randModel = rand.Next(npcModels.Count);
                if (ignoreNPC.Contains(i)) { continue; }
                itemFile[i] = $"\tAssetFilename = \"data/models/npcs/{npcModels[randModel]}\"";
                Debug.WriteLine(itemFile[i]);
                npcModels.Remove(npcModels[randModel]);
                if (randModel <= 0)
                {
                    npcModels.AddRange(File.ReadAllLines($"{Application.StartupPath}\\Resources\\AllNPCModels.txt"));
                }
            }
        }
    }
    private void RandomizeUnstableStats(string[] itemFile, string[] missionFile)
    {
        //returns if both the unstable check boxes are unchecked
        if (!cb_US_NPCItems.Checked && !cb_US_PropToThrow.Checked) { return; }
        var confirmResult = MessageBox.Show
            ("Using any of the unstable stats as a higher chance of soft locking and crashing the game." +
            "\n\n\t\t Are sure you want to continue?", "Warning", MessageBoxButtons.YesNo);
        if (confirmResult == DialogResult.No) { return; }
        Random rand = new Random();

        List<string> allItems = new List<string>();

        for (int i = 0; i < allUnStableitemsTable.Rows.Count; i++)
        {
            //converts the checkbox to a bool and checks if its true if
            //it is true it then gets the item name from table on the 2nd
            //column and adds to the allitems list to add to items.txt file
            bool checkState = Convert.ToBoolean(allUnStableitemsTable.Rows[i].ItemArray[0]);
            if (checkState == true)
            {
                allItems.Add(allUnStableitemsTable.Rows[i].ItemArray[1].ToString());
            }
        }
        if (cb_US_PropToThrow.Checked)
        {
            for (int i = 0; i < itemFile.Length; i++)
            {
                if (itemFile[i].Contains(gb_US_PropToThrow.Tag.ToString()))
                {
                    int randItem = rand.Next(allItems.Count);
                    itemFile[i] =
                        itemFile[i].Split('=')[0] + $"= {allItems[randItem]}";
                }
            }
            File.WriteAllLines($"{path}\\items.txt", itemFile);
        }
        if (cb_US_NPCItems.Checked)
        {
            for (int i = 0; i < missionFile.Length; i++)
            {
                if (missionFile[i].Contains(gb_US_NPCItems.Tag.ToString()))
                {
                    int randItem = rand.Next(allItems.Count);
                    missionFile[i] =
                        missionFile[i].Split('=')[0] + $"= {allItems[randItem]}";
                }
            }
            File.WriteAllLines($"{path}\\missions.txt", missionFile);
        }
    }

}