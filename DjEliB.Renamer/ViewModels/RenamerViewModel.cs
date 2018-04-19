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
        private RenamerModel _renamer = new RenamerModel();

        internal bool isCheckingSelectAll;
        internal bool isSelectingAll;

        private string _txtSourceDirectory;

        public string TxtSourceDirectory
        {
            get { return _txtSourceDirectory; }
            set
            {
                _txtSourceDirectory = value;
                _renamer.SourceDirectory = _txtSourceDirectory;
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
                CheckSelectAll();
                _renamer.IsSinglesChecked = _chkbxIsSinglesChecked;
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
                CheckSelectAll();
                _renamer.IsElectroHouseChecked = _chkbxIsElectroHouseChecked;
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
                CheckSelectAll();
                _renamer.IsDjFtpChecked = _chkbxIsDjFtpChecked;
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
                CheckSelectAll();
                _renamer.IsUnderscoreChecked = _chkbxIsUnderscoreChecked;
                RaisePropertyChangedEvent("ChkbxIsUnderscoreChecked");
            }
        }

        private bool _chkbxIsSelectAllChecked;

        public bool ChkbxIsSelectAllChecked
        {
            get { return _chkbxIsSelectAllChecked; }
            set
            {
                _chkbxIsSelectAllChecked = value;
                SelectAll();
                RaisePropertyChangedEvent("ChkbxIsSelectAllChecked");
            }
        }

        private ICommand _getSourceDirectoryCommand;

        public ICommand GetSourceDirectoryCommand
        {
            get
            {
                return _getSourceDirectoryCommand ?? (_getSourceDirectoryCommand = new RelayCommand(x => { GetSourceDirectory(); }));
            }
        }

        private ICommand _closeApplicationCommand;

        public ICommand CloseApplicationCommand
        {
            get
            {
                return _closeApplicationCommand ?? (_closeApplicationCommand = new RelayCommand(x => { CloseApplication(); }));
            }
        }

        private ICommand _renameCommand;

        public ICommand RenameCommand
        {
            get
            {
                return _renameCommand ?? (_renameCommand = new RelayCommand(x => { Rename(); }));
            }
        }

        private ICommand _consolidateCommand;

        public ICommand ConsolidateCommand
        {
            get
            {
                return _consolidateCommand ?? (_consolidateCommand = new RelayCommand(x => { Consolidate(); }));
            }
        }

        public void GetSourceDirectory()
        {
            TxtSourceDirectory = _renamer.GetSourceDirectory();
        }

        public void SelectAll()
        {
            if (!isCheckingSelectAll)
            {
                isSelectingAll = true;
                ChkbxIsSinglesChecked = _chkbxIsSelectAllChecked;
                ChkbxIsElectroHouseChecked = _chkbxIsSelectAllChecked;
                ChkbxIsDjFtpChecked = _chkbxIsSelectAllChecked;
                ChkbxIsUnderscoreChecked = _chkbxIsSelectAllChecked;
                isSelectingAll = false;
            }
        }

        public void CheckSelectAll()
        {
            if (!isSelectingAll)
            {
                isCheckingSelectAll = true;

                if (_chkbxIsSinglesChecked &&
                    _chkbxIsElectroHouseChecked &&
                    _chkbxIsDjFtpChecked &&
                    _chkbxIsUnderscoreChecked)
                {
                    ChkbxIsSelectAllChecked = true;
                }
                else
                {
                    ChkbxIsSelectAllChecked = false;
                }

                isCheckingSelectAll = false;
            }
        }

        public void CloseApplication()
        {
            Application.Current.Shutdown();
        }

        public void Rename()
        {
            _renamer.Rename();
            MessageBox.Show("Finished renaming.");
        }

        public void Consolidate()
        {
            _renamer.Consolidate();
            MessageBox.Show("Finished consolidation.");
        }
    }
}
