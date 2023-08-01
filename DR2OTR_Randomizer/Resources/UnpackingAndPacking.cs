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
            if (!Directory.Exists(workingPath))
            { Directory.CreateDirectory(workingPath); }
            //Copys the datafile.big from the games dicrotory
            //to be modfied then sets workingPath the unpacked folder
            //and returns it to be used for the randomizer
            if (!File.Exists($"{path}\\data\\datafile.big"))
            {   
                MessageBox.Show("Could not find datafile.big. Make sure you have selected the right folder", "Warning");
                return "";
            }
            File.Copy($"{path}\\data\\datafile.big", $"{workingPath}\\datafile.big", true);
            var process = Process.Start($"{Application.StartupPath}\\Resources\\Unpacker\\Gibbed.DeadRising2.Unpack.exe"
                , $"\"{workingPath}\\datafile.big\"");
            process.WaitForExit();
            gamePath = path;
            var result = MessageBox.Show("Would you like to backup the datafile?", "Backup", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                File.Copy($"{workingPath}\\datafile.big", $"{Application.StartupPath}\\datafile(Backup).big", true);
                MessageBox.Show("The backup is located in the unpacked folder inside the Resources folder");
            }
            workingPath = $"{workingPath}\\datafile_big";
            return workingPath;
        }
        public void Pack()
        {
            //run the packer with the datafile folder location as an argument
            var process = Process.Start($"{Application.StartupPath}\\Resources\\Unpacker\\Gibbed.DeadRising2.Pack.exe", $"\"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big\"");
            process.WaitForExit();
            //waits till the packer as done and checks if the datafile as been made
            if (!File.Exists($"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big.big"))
            {
                MessageBox.Show("Could not find datafile", "Warning");
                return;
            }
            //if the gamePath is null or the user picks to not overwright the datafile
            //it will copy and rename the datafile and put it with the exe root
            if(gamePath == null ||
                MessageBox.Show("Would you like to overwrite the datafile in your games dicorty?", "Warning", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                File.Move($"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big.big", $"{Application.StartupPath}\\datafile.big", true);
                MessageBox.Show("The datafile as has been packed and placed with this program exe.", "Packed");
                return;
            }
            File.Copy($"{Application.StartupPath}\\Resources\\Unpacked\\datafile_big.big",$"{gamePath}\\data\\datafile.big", true);
        }
    }
}
