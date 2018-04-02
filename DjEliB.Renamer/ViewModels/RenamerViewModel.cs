using DjEliB.Renamer.Base;
using DjEliB.Renamer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DjEliB.Renamer.ViewModels
{
    public class RenamerViewModel : ObservableObject
    {
        private RenamerModel Renamer = new RenamerModel();
        private string _txtSourceDirectory;

        public string TxtSourceDirectory
        {
            get { return _txtSourceDirectory; }
            set
            {
                _txtSourceDirectory = value;
                RaisePropertyChangedEvent("TxtSourceDirectory");
            }
        }

        public ICommand BrowseButtonClickCommand
        {
            get { return new DelegateCommand(GetSourceDirectory); }
        }

        public void GetSourceDirectory()
        {
            TxtSourceDirectory = Renamer.GetSourceDirectory();
        }
    }
}
