

using DR2OTR_Randomizer.Resources;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace DR2OTR_Randomizer;
/// <summary>
/// TODO:
/// Move the item randomizer over to a data grid view
/// Like the item stat randomizer.
/// 
/// 
/// Need to spell check
/// Refactor and optiomze the program 
/// 
/// 
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


    int[] unSafeLines = {
        17063, 17173, 17833, 17936, 18035, 18131, 18230, 18300, 18390, 18813, 18930, 19011,
        19092, 19152, 19260, 19355, 19436, 19524, 19722, 19872, 19971, 20064, 20152, 20259,
        20372, 20457, 20522, 20617, 20727, 20826, 77883, 77953, 78625, 78689, 78761, };



    LevelsLines levelLines = new LevelsLines();
    string path;
    public F_ItemRandomiser()
    {
        VheicleStatData = statData.GetVheicleStats();
        NPCStatData = statData.GetNPCStats();
        WorldStatsData = statData.GetWorldStats();
        FireArmsStatData = statData.GetFireArmsStats();
        ExplosiveStatData = statData.GetExplosivesStats();
        FoodAndDamageData = statData.GetFoodAndDamageStats();

        DataTable allitemsTable = new DataTable();
        allitemsTable = itemDataTable.SetAllItemData();
        InitializeComponent();

        //use this to catch if the Allitems or npcmodels file is missing
        try
        {
            File.OpenRead($"{Application.StartupPath}\\Resources\\Allitems.txt");
            File.OpenRead($"{Application.StartupPath}\\Resources\\AllNPCModels.txt");
        }
        catch (FileNotFoundException e)
        {
            MessageBox.Show
            ($"{e.FileName} \nIs missing or as been renamed."
            , "WARNING");
            Process.GetCurrentProcess().Kill();
        }
        var dataArray = File.ReadAllLines($"{Application.StartupPath}\\Resources\\Allitems.txt");


        //hides the search tab till the user clicks the searchbox
        tc_Items.TabPages.Remove(tp_Search);
        //Create a new data table to put inside the check list box
        var dt = new DataTable();

        //adds the string for the item name coloum and theck check box to the data table
        dt.Columns.Add("Checked", typeof(bool));
        dt.Columns.Add("Item", typeof(string));





        //User will be able to add and remove items from the file
        //goese though each of the item in the array for the allitems.txt and
        //adds it to the datatable and defaults its check to false
        //foreach (var item in dataArray) dt.Rows.Add(item, false);
        //Commits the items added with the foreach loop
        dt.AcceptChanges();

        //Adds the data from the data table to the check list box for filtering
        clb_SearchResults.DataSource = dt.DefaultView;


        //Gets the string name of the Item in the datatable columns and dislpays that name
        //and sets the Value for each Item in the Check list box the same as the dispaly name
        clb_SearchResults.DisplayMember = "Item";
        clb_SearchResults.ValueMember = "Item";

        //Binds the item beeing checked with the ItemCheck method below
        clb_SearchResults.ItemCheck += clb_SearchResults_ItemCheck;
        dgv_AllItems.DataSource = allitemsTable;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
        var itemStatData = this.VheicleStatData;
        dgv_ItemStatsTable.DataSource = itemStatData;

    }
    private void tc_TabWindowsTabSelect(object sender, EventArgs e)
    {
        dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
    }
    private void b_DeselectAll_Click(object sender, EventArgs e)
    {
        CheckedListBox currentListBox = tc_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < currentListBox.Items.Count; i++)
        {
            currentListBox.SetItemChecked(i, false);
        }
    }
    private void bt_IS_UnstableUncheck_Click(object sender, EventArgs e)
    {
        CheckedListBox checkedListBox = tc_US_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < checkedListBox.Items.Count; i++)
        {
            checkedListBox.SetItemChecked(i, false);
        }
    }
    private void b_ToggleAll_Click(object sender, EventArgs e)
    {
        ToogleAllItemsCheckState(tc_Items);
    }
    private void bt_IS_UnstableToggle_Click(object sender, EventArgs e)
    {
        ToogleAllItemsCheckState(tc_US_Items);
    }
    private void tsm_open_Click(object sender, EventArgs e)
    {
        //gets the path of the datafile folder
        fbd_DataFileFolder.ShowDialog(this);
        path = fbd_DataFileFolder.SelectedPath;
    }

    private void b_Randomise_Click(object sender, EventArgs e)
    {

        //make a string list to store all the items in each of the check list boxes
        List<string> allItems = new List<string>();

        GetItemsToRandomize(allItems);

        //Stop here if no items have been checked and added to the list
        if (allItems.Count <= 0) { MessageBox.Show("No items have been selected", "Warning"); return; }
        SoftLockAndCrashPrevent(b_Randomise.Text, allItems);
        RandomizeGameItems(allItems);
    }
    private void bt_ItenStatsSet_Click(object sender, EventArgs e)
    {
        if (!File.Exists($"{path}\\items.txt") || !File.Exists($"{path}\\missions.txt"))
        { MessageBox.Show("Could not find items.txt", "Warning"); return; }
        string[] itemFile = File.ReadAllLines($"{path}\\items.txt");
        string[] missionFile = File.ReadAllLines($"{path}\\missions.txt");
        if (tc_itemStats.SelectedTab.Name == "tp_UnstableStats")
        {
            RandomizeUnstableStats(itemFile, missionFile);
        }
        else
        {
            RandomizeItemStats(itemFile);
        }
        File.WriteAllLines($"{path}\\missions.txt", missionFile);
        SoftLockAndCrashPrevent(bt_ItenStatsSet.Text, null);
        MessageBox.Show("Item stats have successfully been randomized", "Success");
    }
    private void clb_SearchResults_ItemCheck(object sender, ItemCheckEventArgs e)
    {
        //this method will be called each time an item is check

        //sets as DataView so it can be filtered
        var dv = clb_SearchResults.DataSource as DataView;
        //gets the item that was just check or unchecked location
        var drv = dv[e.Index];
        //Gets its current checked state before being clicked then
        //will set its new state after being checked/uncheck
        drv["Checked"] = e.NewValue == CheckState.Checked ? true : false;
    }
    private void tb_ItemsSearch_TextChanged(object sender, EventArgs e)
    {
        var dv = clb_SearchResults.DataSource as DataView;

        //Makes the filter using a check text box by getting the text in the check box
        //then using the Like Wildcard by compareing the items
        //in the Data Source to the text typed in to the text box
        var filter = tb_ItemsSearch.Text.Trim().Length > 0
        ? $"Item LIKE '*{tb_ItemsSearch.Text}*'"
        : null;

        //applys the filter to the data source
        dv.RowFilter = filter;
        for (var i = 0; i < clb_SearchResults.Items.Count; i++)
        {
            //gets theitems check state and restores it first it gets the item
            //then it Converts the DataRow Called "Checked" to a bool while
            //storeing its state and apllying it back
            var drv = clb_SearchResults.Items[i] as DataRowView;
            var chk = Convert.ToBoolean(drv["Checked"]);
            clb_SearchResults.SetItemChecked(i, chk);
        }

    }
    private void tb_ItemsSearch_Click(object sender, EventArgs e)
    {
        //displays the search tab if its not already displayed and selects it
        if (!tp_Search.Created)
        {
            tc_Items.TabPages.Insert(0, tp_Search);
            tc_Items.SelectTab(tp_Search);
        }
    }
    private void safeModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        safeMode = !safeMode;
        if (safeMode)
        {
            l_SafeMode_Text.Text = "Safe Mode Is Enabled";
            l_SafeMode_Text.ForeColor = Color.Green;
            safeModeToolStripMenuItem.Text = "Safe Mode Enabled";
        }
        else
        {
            MessageBox.Show("Disabling safe mode will randomize more areas but with a much high chance of crashing", "WARNING");
            safeModeToolStripMenuItem.Text = "Safe Mode Disabled";
            l_SafeMode_Text.Text = "Safe Mode Is Disabled";
            l_SafeMode_Text.ForeColor = Color.Red;
        }
    }
    private void bt_NPC_Model_Randomizer_Click(object sender, EventArgs e)
    {
        if (!File.Exists($"{path}\\items.txt")) { MessageBox.Show("Could not find items.txt", "Warning"); return; }
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


        switch (tc_Items.SelectedTab.Name)
        {
            case "tp_AllItems":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = null;
                break;
            case "tp_BasicCombo":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Basic Combo'";
                break;
            case "tp_BasicFood":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Basic Food'";
                break;
            case "tp_BasicLarge":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Basic Large'";
                break;
            case "tp_BasicSmall":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Basic Small'";
                break;
            case "tp_Bugged":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Bugged'";
                break;
            case "tp_Clothing":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Clothing'";
                break;
            case "tp_CombinedFireArmsSpray":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'CombinedFireArmsSpray'";
                break;
            case "tp_CombinedFoodSpoiled":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'CombinedFoodSpoiled'";
                break;
            case "tp_CombinedThowingMelee":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'CombinedThowingMelee'";
                break;
            case "tp_ComboFireArmSpray":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'ComboFireArmSpray'";
                break;
            case "tp_DLC":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'DLC'";
                break;
            case "tp_Explosive":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Explosive'";
                break;
            case "tp_KeyItems":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'KeyItems'";
                break;
            case "tp_Magazines":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Magazines'";
                break;
            case "tp_Mannequin":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Mannequin'";
                break;
            case "tp_PushPlaced":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'PushPlaced'";
                break;
            case "tp_Special":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Special'";
                break;
            case "tp_Vehicles":
                (dgv_AllItems.DataSource as DataTable).DefaultView.RowFilter = "ItemTag = 'Vehicles'";
                break;

        }
    }
    private void tc_itemStats_SelectedTab(object sender, EventArgs e)
    {
        //when changing tab the selected cell will move over 1 so there is no issues with tolggle them
        switch (tc_itemStats.SelectedTab.Name)
        {
            case "tp_VehicleStats":
                dgv_ItemStatsTable.DataSource = VheicleStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_NPC":
                dgv_ItemStatsTable.DataSource = NPCStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_FireArms":
                dgv_ItemStatsTable.DataSource = FireArmsStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_WorldStats":
                dgv_ItemStatsTable.DataSource = WorldStatsData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_ExplosivesSpray":
                dgv_ItemStatsTable.DataSource = ExplosiveStatData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
            case "tp_FoodDamage":
                dgv_ItemStatsTable.DataSource = FoodAndDamageData;
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
                break;
        }
        if (tc_itemStats.SelectedTab.Name == "tp_UnstableStats")
        { dgv_ItemStatsTable.Visible = false; }
        else { dgv_ItemStatsTable.Visible = true; }
    }

    private void dgv_ItemStatsTable_ColumnHeaderClicked(
        object sender, DataGridViewCellMouseEventArgs e)
    {
        if (dgv_ItemStatsTable.Columns[0].DataPropertyName == "StatState")
        {
            foreach (DataGridViewRow row in dgv_ItemStatsTable.Rows)
            {
                var objectToBool = Convert.ToBoolean(row.Cells[0].Value);
                objectToBool = !objectToBool;
                row.Cells[0].Value = objectToBool;
                //dgv_ItemStatsTable.RefreshEdit();
            }
        }
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
                dgv_ItemStatsTable.CurrentCell = dgv_ItemStatsTable.Rows[0].Cells[1];
            }
        }
    }

    private void bt_IS_CheckAllActiveTab_Click(object sender, EventArgs e)
    {
        CheckAllItemsStatsInTab();
    }
    private void tsm_Quit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
    private void SoftLockAndCrashPrevent(string buttonClicked, List<string> Allitems)
    {
        List<string> missionFile = new List<string>();
        bool showonce = false;
        missionFile.AddRange(File.ReadAllLines($"{path}\\missions.txt"));
        //This will stop overtime softlocking if the player reloads while in overtime
        if (buttonClicked == b_Randomise.Text)
        {
            foreach (string item in clb_KeyItems.Items)
            {
                if (Allitems.Contains(item) && missionFile[50046] != "")
                {
                    missionFile[50046] = "";
                }
            }
            foreach (string item in clb_Vehicles.Items)
            {
                if (Allitems.Contains(item) && !showonce)
                {
                    MessageBox.Show("Having too many vehicles in one area can cause the game to be unstable or crash", "Warning");
                    showonce = true;
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
    private void CheckAllItemsStatsInTab()
    {
        foreach (DataGridViewRow row in dgv_ItemStatsTable.Rows)
        {
            var objectToBool = Convert.ToBoolean(row.Cells[0].Value);
            objectToBool = !objectToBool;
            row.Cells[0].Value = objectToBool;
        }
    }
    private void ToogleAllItemsCheckState(TabControl tabControl)
    {
        CheckedListBox currentListBox = tabControl.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < currentListBox.Items.Count; i++)
        {
            //Gets the check sate and then converts it to a bool for a toggle
            var checkToBool = Convert.ToBoolean(currentListBox.GetItemCheckState(i));
            checkToBool = !checkToBool;
            currentListBox.SetItemChecked(i, checkToBool);
        }
    }
    private void GetItemsToRandomize(List<string> allItems)
    {
        //make a list of type check list box to store the check boxes inside of the tab control
        List<CheckedListBox> allCheckboxes = new List<CheckedListBox>();
        //go though each tab page in the tab control
        foreach (TabPage tabPage in tc_Items.TabPages)
        {
            if (tabPage.Controls.OfType<CheckedListBox>() != null)
            {
                //looks for a check list box inside of the tab page and then adds it to the check list box list
                allCheckboxes.Add(tabPage.Controls.OfType<CheckedListBox>().First());
            }
        }
        foreach (CheckedListBox item in allCheckboxes)
        {
            if (item.Name != "clb_SearchResults")
            {
                //once it has checked all of the tab pages and added all the list check boxes to the list
                foreach (string itemName in item.CheckedItems)
                {
                    //look though each of the checklistbox in the list adds all items that are checked
                    allItems.Add(itemName);
                }
            }
            if (item.Name == "clb_SearchResults")
            {
                foreach (DataRowView itemName in clb_SearchResults.CheckedItems)
                {
                    //Because im using a data table for the search clb i have to 
                    //get the item name from the DataRow with the row name "Item"
                    allItems.Add(itemName.Row["Item"].ToString());
                }
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
                return;
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

        var itemStatLists = new Dictionary<List<ItemStatsData>, bool>
        {
            { VheicleStatData, true},
            { NPCStatData, true },
            { FireArmsStatData, true},
            { WorldStatsData, true },
            { ExplosiveStatData, true },
            { FoodAndDamageData, true },
        };

        foreach (List<ItemStatsData> itemStats in itemStatLists.Keys)
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
        var confirmResult = MessageBox.Show
            ("Enableing any of the unstable stats can cause soft locking and crashing" +
            "Are sure you want to continue?", "Warning", MessageBoxButtons.YesNo);
        if (confirmResult == DialogResult.No) { return; }
        Debug.WriteLine("It has passed the message box");
        Random rand = new Random();

        List<CheckedListBox> allCheckedListBoxes = new List<CheckedListBox>();
        List<string> allItems = new List<string>();

        foreach (TabPage tabpage in tc_US_Items.Controls)
        {
            allCheckedListBoxes.Add(tabpage.Controls.OfType<CheckedListBox>().First());
        }
        foreach (CheckedListBox checkedListBox in allCheckedListBoxes)
        {
            foreach (string item in checkedListBox.CheckedItems)
            {
                allItems.Add(item);
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
    private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
        if (dgv_ItemStatsTable.CurrentCell.ColumnIndex == 3 ||
            dgv_ItemStatsTable.CurrentCell.ColumnIndex == 4)
        {
            TextBox tb = e.Control as TextBox;
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
}