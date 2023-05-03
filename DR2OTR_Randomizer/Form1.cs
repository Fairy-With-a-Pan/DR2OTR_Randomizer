

using DR2OTR_Randomizer.Resources;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.AccessControl;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace DR2OTR_Randomizer;
/// <summary>
/// TODO:
/// Need to spell check
/// 
/// Moving the Item Stats to a Data View Grid need to use a for loop
/// to get the Enabled Colloum and check of that is enabled else skip it
/// 
/// Refactor and optiomze the program 
/// 
/// 
/// 
/// </summary>
public partial class F_ItemRandomiser : Form
{
    public List<ItemStatsData> ItemStats { get; set; }
    bool safeMode = true;

    int[] unSafeLines = {
        17063, 17173, 17833, 17936, 18035, 18131, 18230, 18300, 18390, 18813, 18930, 19011,
        19092, 19152, 19260, 19355, 19436, 19524, 19722, 19872, 19971, 20064, 20152, 20259,
        20372, 20457, 20522, 20617, 20727, 20826, 77883, 77953, 78625, 78689, 78761, };

    LevelsLines levelLines = new LevelsLines();

    string path;
    public F_ItemRandomiser()
    {
        ItemStats = GetItemData();
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
        //string[] npcModels = File.ReadAllLines($"{Application.StartupPath}\\Resources\\AllNPCModels.txt");

        //hides the search tab till the user clicks the searchbox
        tc_Items.TabPages.Remove(tp_Search);
        //Create a new data table to put inside the check list box
        var dt = new DataTable();

        //adds the string for the item name coloum and theck check box to the data table
        dt.Columns.Add("Item", typeof(string));
        dt.Columns.Add("Checked", typeof(bool));

        //NEEDS TO SHIP WITH THE "Allitems.txt"
        //User will be able to add and remove items from the file
        //goese though each of the item in the array for the allitems.txt and
        //adds it to the datatable and defaults its check to false
        foreach (var item in dataArray) dt.Rows.Add(item, false);
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
    }
    private List<ItemStatsData> GetItemData()
    {
        var list = new List<ItemStatsData>();
        list.Add(new ItemStatsData()
        {
            StatState = true,
            StatName = "Vehicle Air Density:",
            StatDescription = "Controls how much air resistance there is for the vehicle. The lower the less air resistance.",
            StatMin = 0,
            StatMax = 10,
            StatInGameName = "\tAirDensity"

        });
        list.Add(new ItemStatsData()
        {
            StatState = false,
            StatName = "Vehicle Max RPM:",
            StatDescription = "One of the stats that controls the vehicles speed. May start to auto acelerate at higher values.",
            StatMin = 3550,
            StatMax = 10000,
            StatInGameName = "\tEngine_MaxRPM"
        });
        list.Add(new ItemStatsData()
        {
            StatState = true,
            StatName = "Vehicle Min RPM",
            StatDescription = "Gives a burst of speed when first acelerating. Setting higher can cause it to auto acelerate.",
            StatMin = 885,
            StatMax = 5000,
            StatInGameName = "\tEngine_MinRPM"

        });
        return list;
    }
    private void Form1_Load(object sender, EventArgs e)
    {
        var itemStatData = this.ItemStats;
        dataGridView1.DataSource = itemStatData;
    }
    private void b_DeselectAll_Click(object sender, EventArgs e)
    {
        CheckedListBox currentListBox = tc_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < currentListBox.Items.Count; i++)
        {
            currentListBox.SetItemChecked(i, false);
        }
    }

    private void b_ToggleAll_Click(object sender, EventArgs e)
    {
        CheckedListBox currentListBox = tc_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < currentListBox.Items.Count; i++)
        {
            //Gets the check sate and then converts it to a bool for a toggle
            var checkToBool = Convert.ToBoolean(currentListBox.GetItemCheckState(i));
            checkToBool = !checkToBool;
            currentListBox.SetItemChecked(i, checkToBool);
        }

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
    private void tc_itemStats_SelectedTab(object sender, EventArgs e)
    {
        if (tc_itemStats.SelectedTab.Text == "Unstable Stats")
        {
            bt_IS_CheckAllActiveTab.Text = "Toggle all itmes in Active tab";
        }
        else
        {
            bt_IS_CheckAllActiveTab.Text = "Toggle all stats in Active tab";
        }
    }

    private void bt_IS_CheckAllActiveTab_Click(object sender, EventArgs e)
    {
        //CheckAllItemsinTab();
        NewItemSataTesting();
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
    private void CheckAllItemsinTab()
    {
        TabPage activePage = tc_itemStats.SelectedTab;

        List<CheckBox> allActiveCheckBoxes = new List<CheckBox>();
        if (activePage.Name == "tp_UnstableStats")
        {
            CheckedListBox currentCheckListBox =
                tc_US_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
            for (int i = 0; i < currentCheckListBox.Items.Count; i++)
            {
                var checkToBool = Convert.ToBoolean(currentCheckListBox.GetItemCheckState(i));
                checkToBool = !checkToBool;
                currentCheckListBox.SetItemChecked(i, checkToBool);
            }
        }
        else
        {
            foreach (GroupBox tab in activePage.Controls)
            {
                allActiveCheckBoxes.Add(tab.Controls.OfType<CheckBox>().First());
            }
            foreach (CheckBox checkBox in allActiveCheckBoxes)
            {
                checkBox.Checked = !checkBox.Checked;
            }
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

        List<GroupBox> allGroupBoxes = new List<GroupBox>();

        foreach (TabPage tabpage in tc_itemStats.TabPages)
        {
            if (tabpage.Name != "tp_UnstableStats")
            {
                foreach (GroupBox groupBox in tabpage.Controls)
                { allGroupBoxes.Add(groupBox); }
            }
        }
        foreach (GroupBox box in allGroupBoxes)
        {
            if (box.Controls.OfType<CheckBox>().First().Checked)
            {
                decimal dNum1 = box.Controls.OfType<NumericUpDown>().First().Value;
                decimal dNum2 = box.Controls.OfType<NumericUpDown>().Last().Value;
                int[] numbs = { Convert.ToInt32(dNum1), Convert.ToInt32(dNum2) }; Array.Sort(numbs);

                for (int i = 0; i < itemFile.Length; i++)
                {
                    if (itemFile[i].Contains(box.Tag.ToString()))
                    {
                        if (unSafeLines.Contains(i + 1) && safeMode) { continue; }
                        int randStatNumb = rand.Next(numbs[0], numbs[1]);
                        itemFile[i] =
                            itemFile[i].Split('=')[0] + $"= \"{randStatNumb}\"";
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
    private void NewItemSataTesting()
    {
        //Debug.WriteLine(dataGridView1.Rows[2].Cells[2].Value);

        foreach (DataGridViewRow data in dataGridView1.Rows)
        {
            foreach(DataGridViewCell cell in data.Cells)
            {
                if(cell.OwningColumn.HeaderText == "Enabled")
                {
                    Debug.WriteLine(cell.OwningColumn.HeaderText);
                    Debug.WriteLine(cell.Value);
                }

            }
        }
    }

}