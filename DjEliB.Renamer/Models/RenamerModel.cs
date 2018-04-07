using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DjEliB.Renamer.Models
{
    public class RenamerModel
    {
        public string SourceDirectory { get; set; }
        public bool IsSinglesChecked { get; set; }
        public bool IsElectroHouseChecked { get; set; }
        public bool IsDjFtpChecked { get; set; }
        public bool IsUnderscoreChecked { get; set; }

        public string GetSourceDirectory()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    SourceDirectory = dialog.SelectedPath;
                }
            }

            return SourceDirectory;
        }
    }
}
