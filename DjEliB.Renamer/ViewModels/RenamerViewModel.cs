using DjEliB.Renamer.Base;
using DjEliB.Renamer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        
        private bool _chkbxIsSinglesChecked;

        public bool ChkbxIsSinglesChecked
        {
            get { return _chkbxIsSinglesChecked; }
            set
            {
                _chkbxIsSinglesChecked = value;
                RaisePropertyChangedEvent("ChkbxIsSinglesChecked");
            }
        }

        private bool _chkbxIsElectroHouseChecked;

        public bool ChkbxIsElectroHouseChecked
        {
            get { return _chkbxIsElectroHouseChecked; }
            set
            {
                _chkbxIsElectroHouseChecked = value;
                RaisePropertyChangedEvent("ChkbxIsElectroHouseChecked");
            }
        }

        private bool _chkbxIsDjFtpChecked;

        public bool ChkbxIsDjFtpChecked
        {
            get { return _chkbxIsDjFtpChecked; }
            set
            {
                _chkbxIsDjFtpChecked = value;
                RaisePropertyChangedEvent("ChkbxIsDjFtpChecked");
            }
        }

        private bool _chkbxIsUnderscoreChecked;

        public bool ChkbxIsUnderscoreChecked
        {
            get { return _chkbxIsUnderscoreChecked; }
            set
            {
                _chkbxIsUnderscoreChecked = value;
                RaisePropertyChangedEvent("ChkbxIsUnderscoreChecked");
            }
        }

        private bool _chkbxIsSelectAllChecked;

        public bool ChkbxIsSelectAllChecked
        {
            get {
                return _chkbxIsSelectAllChecked;
            }
            set
            {
                _chkbxIsSelectAllChecked = value;
                RaisePropertyChangedEvent("ChkbxIsSelectAllChecked");
            }
        }

        private ICommand _getSourceDirectoryCommand;

        public ICommand GetSourceDirectoryCommand
        {
            get
            {
                return _getSourceDirectoryCommand ?? (_getSourceDirectoryCommand = new RelayCommand(x => {GetSourceDirectory();}));
            }
        }

        //private ICommand _selectAllCommand;

        //public ICommand SelectAllCommand
        //{
        //    get
        //    {
        //        return _selectAllCommand ?? (_selectAllCommand = new RelayCommand(x => { SelectAll(); }));
        //    }
        //}

        public void GetSourceDirectory()
        {
            TxtSourceDirectory = Renamer.GetSourceDirectory();
        }

        //public void SelectAll()
        //{
        //    if (!_chkbxIsSinglesChecked)
        //    {
        //        ChkbxIsSinglesChecked = true;
        //    }
        //}
    }
}
