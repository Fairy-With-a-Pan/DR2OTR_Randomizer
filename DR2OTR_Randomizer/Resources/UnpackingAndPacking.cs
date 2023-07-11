using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DR2OTR_Randomizer.Resources
{
    internal class UnpackingAndPacking
    {
        public string workingPath = $"{Application.StartupPath}Resources\\Unpacked\\";
        string gamePath;
        public string Unpack(string path)
        {
            Debug.WriteLine(workingPath);
            //Copys the datafile.big from the games dicrotory
            //to be modfied then sets workingPath the unpacked folder
            //and returns it to be used for the randomizer
            File.Copy($"{path}\\data\\datafile.big", $"{workingPath}\\datafile.big", true);
            var process = Process.Start($"{Application.StartupPath}\\Resources\\Unpacker\\Gibbed.DeadRising2.Unpack.exe"
                , $"\"{workingPath}\\datafile.big\"");
            process.WaitForExit();
            gamePath = path;
            var result = MessageBox.Show("Would you like to back up the datafile?", "Backup", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                File.Copy($"{workingPath}\\datafile.big", $"{Application.StartupPath}\\datafile(Backup).big", true);
                MessageBox.Show("The back up is located in the unpacked folder inside the Resources folder");
            }
            workingPath = $"{workingPath}\\datafile_big";
            return workingPath;
        }
        public void Pack()
        {
            var process = Process.Start($"{Application.StartupPath}\\Resources\\Unpacker\\Gibbed.DeadRising2.Pack.exe", $"\"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big\"");
            process.WaitForExit();
            if (!File.Exists($"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big.big"))
            {
                MessageBox.Show("Could not find datafile", "Warning");
                return;
            }
            var result = MessageBox.Show("This will overwrite the datafile inside of the games folder", "Warning", MessageBoxButtons.YesNo);
            if (result == DialogResult.No) 
            {
                MessageBox.Show("The datafile as been packed and is located in the unpacked folder inside the resources folder. " +
                    "You will need to rename it to \"datafile.big\" before adding it to your games directory");
                return;
            }
            File.Copy($"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big.big",$"{gamePath}\\data\\datafile.big", true);
        }
    }
}
