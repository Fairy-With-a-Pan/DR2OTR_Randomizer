using MaterialSkin;
using MaterialSkin.Controls;

namespace DR2OTR_Randomizer;

public partial class Form1 : MaterialForm
{
    public Form1()
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
}
