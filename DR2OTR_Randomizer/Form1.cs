

using DR2OTR_Randomizer.Resources;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace DR2OTR_Randomizer;

public partial class F_ItemRandomiser : Form
{
    LevelsLines levelLines = new LevelsLines();
    List <string> searchItems = new List <string> ();
    public F_ItemRandomiser()
    {
        InitializeComponent();
        tc_Items.TabPages.Remove(tp_Search);
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
    
    string path;
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
        foreach (TabPage items in tc_Items.TabPages)
        {
            if (items.Controls.OfType<CheckedListBox>() != null)
            {
                //looks for a check list box inside of the tab page and then adds it to the check list box list
                allCheckboxes.Add(items.Controls.OfType<CheckedListBox>().First());
            }
        }
        foreach (CheckedListBox item in allCheckboxes)
        {
            //once it has checked all of the tab pages and added all the list check boxes to the list
            foreach (string itemName in item.CheckedItems)
            {
                //look though each of the checklistbox in the list adds all items that are checked
                allItems.Add(itemName);
                if(clb_Vehicles.CheckedItems.Count >= 1 && !vehiclesWarning)
                {
                    MessageBox.Show("Having too many vehicles in one area can cause the game to be unstable or crash", "Warning");
                    vehiclesWarning = true;
                }
            }
        }
        //Stop here if no items have been checked and added to the list
        if(allItems.Count <= 0) { MessageBox.Show("No items have been selected", "Warning");return; }


        //gets the dictionary stored in the LevelLines class
        var levels = LevelsLines.levels;
        Random rand = new Random();
        //goese though each of the levels in side of the dictionary
        foreach (var level in levels)
        {
            foreach(string file in files)
            {
                //checks to see that the selected path has all the requied files
                if (!File.Exists($"{path}\\{level.Value}")  )
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

    private void tsm_Quit_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }
    private void tb_ItemsSearch_Click(object sender, EventArgs e)
    {
        if(tb_ItemsSearch.Text == "Search Items" && !tp_Search.Created)
        {
            tb_ItemsSearch.Text = "";
            tc_Items.TabPages.Insert(0,tp_Search);
            tc_Items.SelectTab(tp_Search);
            AddAllItemsToList();
            foreach (string item in searchItems)
            {
                clb_SearchResults.Items.Add(item);
            }
        }
    }
    private void tb_ItemsSearch_TextChanged(object sender, EventArgs e)
    {
        ////NOT WORKING NEED TO FIND A WAY TO STOP DUPLICATE ITEMS AND KEEP THE CHECKED ITEMS CHECKED


        //List<string> _checkedItems = new List<string>();
        //List<string> _uncheckedItems = new List<string>();
        ////List<string> testing = new List<string>();
        ////testing = searchItems;
        //foreach (string item in searchItems)
        //{
        //    if(!item.Contains(tb_ItemsSearch.Text, StringComparison.OrdinalIgnoreCase))
        //    {
        //        //if (clb_SearchResults.Items.Cast<string>().Contains(item)) { clb_SearchResults.Items.Remove(item); }
        //        if (clb_SearchResults.CheckedItems.Contains(item))
        //        {
        //            _checkedItems.Add(item);
        //        }  
        //        if (!clb_SearchResults.CheckedItems.Contains(item))
        //        {
        //            _uncheckedItems.Add(item);
        //        }
        //        clb_SearchResults.Items.Remove(item);
        //    }
        //    if (item.Contains(tb_ItemsSearch.Text, StringComparison.OrdinalIgnoreCase))
        //    {
        //                        if (clb_SearchResults.CheckedItems.Contains(item))
        //        {
        //            _checkedItems.Add(item);
        //        }  
        //        if (!clb_SearchResults.CheckedItems.Contains(item))
        //        {
        //            _uncheckedItems.Add(item);
        //        }
        //        clb_SearchResults.Items.Remove(item);
        //    }

        //}
        //List<string>checkedItems = _checkedItems.Distinct().ToList();
        //List<string>uncheckedItems = _uncheckedItems.Distinct().ToList();
        //_checkedItems.Clear();
        //_uncheckedItems.Clear();
        //Debug.WriteLine($"the number of checked items is : {checkedItems.Count}\n and {uncheckedItems.Count}");
        //if (tb_ItemsSearch.Text == "retry")
        //{
        //    for(int i = 0; i < checkedItems.Count;i++)
        //    {
        //        clb_SearchResults.Items.Add(checkedItems[i]);
        //    }
        //    for(int i = 0; i < clb_SearchResults.Items.Count; i++) { clb_SearchResults.SetItemChecked(i, true); }
        //    for(int i = 0; i < uncheckedItems.Count; i++)
        //    {
        //        clb_SearchResults.Items.Add(uncheckedItems[i]);
        //    }
        //    checkedItems.Clear();
        //    uncheckedItems.Clear();
        //    //for(int i =0; i < testing.Count;i++)
        //    //{
        //    //    clb_SearchResults.Items.Insert(0, testing[i]);
        //    //}
        //    ////testing.Clear();
        //}
        if (clb_SearchResults.Items.Count > 800)  { MessageBox.Show("ITEMS HAVE BEEN DUPLATED", "WARNING"); }
    }
    private void AddAllItemsToList()
    {
        foreach(TabPage page in tc_Items.TabPages)
        {
            if(page != tp_Search)
            {
                CheckedListBox box = page.Controls.OfType<CheckedListBox>().First();
                
                foreach(string item in box.Items)
                {
                    searchItems.Add(item);
                }
            }
        }
    }
}
