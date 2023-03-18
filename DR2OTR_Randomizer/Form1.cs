using MaterialSkin;
using MaterialSkin.Controls;

namespace DR2OTR_Randomizer;

public partial class F_ItemRandomiser : MaterialForm
{
    public F_ItemRandomiser()
    {
        InitializeComponent();
        var materialSkinManager = MaterialSkinManager.Instance;
        materialSkinManager.AddFormToManage(this);
        materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
        materialSkinManager.ColorScheme = new ColorScheme(Primary.Grey800, Primary.Grey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
    }

    private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void Form1_Load(object sender, EventArgs e)
    {

    }

    private void materialCheckedListBox1_Paint(object sender, PaintEventArgs e)
    {

    }

    private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
    {
        
    }

    private void TV_ListOfItems_AfterSelect(object sender, TreeViewEventArgs e)
    {

    }
}
