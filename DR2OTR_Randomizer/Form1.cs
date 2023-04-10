

using DR2OTR_Randomizer.Resources;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace DR2OTR_Randomizer;

public partial class F_ItemRandomiser : Form
{
    LevelsLines levelLines = new LevelsLines();

    string path;
    public F_ItemRandomiser()
    {
        InitializeComponent();
        //use this to catch if the Allitems text file is missingq
        if ($"{Application.StartupPath}\\Allitems.txt" == null)
        {
            MessageBox.Show
            ("The \"Allitems.txt\" is missing please make sure it " +
            "is in the same location as this programs exe "
            , "WARNING");
            Process.GetCurrentProcess().Kill();
        }

        var dataArray = File.ReadAllLines($"{Application.StartupPath}\\Allitems.txt");
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

    string[] files =
    {
        "americana_casino.txt",
        "arena_backstage.txt",
        "atlantica_casino.txt",
        "food_barn.txt",
        "fortune_exterior.txt",
        "items.txt",
        "laboratory.txt",
        "missions.txt",
        "palisades.txt",
        "rooftop_atlantica.txt",
        "rooftop_hotel.txt",
        "rooftop_royal.txt",
        "rooftop_safehouse.txt",
        "rooftop_theater.txt",
        "rooftop_yucatan.txt",
        "royal_flush.txt",
        "safehouse.txt",
        "south_plaza.txt",
        "tape_die.txt",
        "theme_park.txt",
        "tkot_battle.txt",
        "underground.txt",
        "yucatan_casino.txt"
    };

    private void Form1_Load(object sender, EventArgs e)
    {

    }
    private void tc_Items_Click(object sender, EventArgs e)
    {

    }



    private void b_DeselectAll_Click(object sender, EventArgs e)
    {
        CheckedListBox currentListBox = tc_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < currentListBox.Items.Count; i++)
        {
            currentListBox.SetItemChecked(i, false);
        }
    }

    private void b_CheckAll_Click(object sender, EventArgs e)
    {

        CheckedListBox currentListBox = tc_Items.SelectedTab.Controls.OfType<CheckedListBox>().First();
        for (int i = 0; i < currentListBox.Items.Count; i++)
        {
            currentListBox.SetItemChecked(i, true);
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

        bool vehiclesWarning = false;

        //make a list of type check list box to store the check boxes inside of the tab control
        List<CheckedListBox> allCheckboxes = new List<CheckedListBox>();
        //make a string list to store all the items in each of the check list boxes
        List<string> allItems = new List<string>();
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
                    if (clb_Vehicles.CheckedItems.Count >= 1 && !vehiclesWarning)
                    {
                        MessageBox.Show("Having too many vehicles in one area can cause the game to be unstable or crash", "Warning");
                        vehiclesWarning = true;
                    }
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
        //Stop here if no items have been checked and added to the list
        if (allItems.Count <= 0) { MessageBox.Show("No items have been selected", "Warning"); return; }


        //gets the dictionary stored in the LevelLines class
        var levels = LevelsLines.levels;
        Random rand = new Random();
        //goese though each of the levels in side of the dictionary
        foreach (var level in levels)
        {
            foreach (string file in files)
            {
                //checks to see that the selected path has all the requied files
                if (!File.Exists($"{path}\\{level.Value}"))
                {
                    //returns if it cant find any
                    MessageBox.Show($"Could not find {level.Value} please check your datafile folder", "Warning");
                    return;
                }
            }
            //gets the current level file in the dictionary with the level.Value is the same as the files name
            string[] levelFile = File.ReadAllLines($"{path}\\{level.Value}");
            //adds all the lines inside of the current selected level to an array
            foreach (int lines in level.Key)
            {
                //gets each line that needs to be changed by getting the dictionary key array witch as all the lines inside
                int item = rand.Next(allItems.Count);
                //changes the line by looking for the = inside of the string then
                //rewiting all text after it with the current pick item
                levelFile[lines - 1] =
                    levelFile[lines - 1].Split('=')[0] + $"= {allItems[item]}";

            }
            //Writes all the lines inside of the levelfile array to the levels txt file
            File.WriteAllLines($"{path}\\{level.Value}", levelFile);

        }
        MessageBox.Show("All levels successfully randomised with selected items", "Success");
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
    private void tsm_Quit_Click(object sender, EventArgs e)
    {
        Application.Exit();
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
    private void bt_ItenStatsSet_Click(object sender, EventArgs e)
    {
        if (!File.Exists($"{path}\\items.txt")) { MessageBox.Show("Could not find items.txt", "Warning"); return; }
        string[] itemFile = File.ReadAllLines($"{path}\\items.txt");

        Random rand = new Random();

        List<GroupBox> allGroupBoxes = new List<GroupBox>();

        foreach (TabPage tabpage in tc_itemStats.TabPages)
        {
            foreach (GroupBox groupBox in tabpage.Controls)
            { allGroupBoxes.Add(groupBox); }
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
                        int randStatNumb = rand.Next(numbs[0], numbs[1]);
                        itemFile[i] =
                            itemFile[i].Split('=')[0] + $"= \"{randStatNumb}\"";
                    }
                }
            }
        }
        File.WriteAllLines($"{path}\\items.txt", itemFile);
        MessageBox.Show("Item stats have successfully been randomized", "Success");
    }

    private void bt_IS_CheckAllActiveTab_Click(object sender, EventArgs e)
    {
        TabPage activePage = tc_itemStats.SelectedTab;

        List<CheckBox> allActiveCheckBoxes = new List<CheckBox>();

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