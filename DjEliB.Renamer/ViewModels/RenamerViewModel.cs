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

        public ICommand BrowseButtonCommand { get; set; }
        public ICommand ConsolidateFilesButtonCommand { get; set; }
        public ICommand RenameButtonCommand { get; set; }

        public RenamerViewModel()
        {
            BrowseButtonCommand = new RelayCommand(OnBrowseButton);
            ConsolidateFilesButtonCommand = new RelayCommand(OnConsolidateFilesButton);
            RenameButtonCommand = new RelayCommand(OnRenameButton);
            ResetMainProgress();
        }

        private async void OnRenameButton(object param)
        {
            await Task.Run(() => Rename());
        }

        private void OnBrowseButton(object param)
        {
            GetSourceDirectory();
        }

        private async void OnConsolidateFilesButton(object param)
        {
            MainProgressIsIndeterminate = true;
            await Task.Run(() => Consolidate());
            MainProgressIsIndeterminate = false;
        }

        private bool _mainProgressIsIndeterminate;

        public bool MainProgressIsIndeterminate
        {
            get { return _mainProgressIsIndeterminate; }
            set
            {
                if (_mainProgressIsIndeterminate != value)
                {
                    _mainProgressIsIndeterminate = value;
                    NotifyPropertyChanged("MainProgressIsIndeterminate");
                }
            }
        }

        private int _mainProgressValue;

        public int MainProgressValue
        {
            get { return _mainProgressValue; }
            set
            {
                if (_mainProgressValue != value)
                {
                    _mainProgressValue = value;
                    NotifyPropertyChanged("MainProgressValue");
                }
            }
        }

        private int _mainProgressMax;

        public int MainProgressMax
        {
            get { return _mainProgressMax; }
            set
            {
                if (_mainProgressMax != value)
                {
                    _mainProgressMax = value;
                    NotifyPropertyChanged("MainProgressMax");
                }
            }
        }

        private string _txtSourceDirectory;

        public string TxtSourceDirectory
        {
            get { return _txtSourceDirectory; }
            set
            {
                _txtSourceDirectory = value;
                _renamer.SourceDirectory = _txtSourceDirectory;
                NotifyPropertyChanged("TxtSourceDirectory");
            }
        }

        private string _txtReleaseName;

        public string TxtReleaseName
        {
            get { return _txtReleaseName; }
            set
            {
                _txtReleaseName = value;
                _renamer.ReleaseName = _txtReleaseName;
                NotifyPropertyChanged("TxtReleaseName");
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
                NotifyPropertyChanged("ChkbxIsSinglesChecked");
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
                NotifyPropertyChanged("ChkbxIsElectroHouseChecked");
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
                NotifyPropertyChanged("ChkbxIsDjFtpChecked");
            }
        }

        private bool _chkbxIsZeroDayMusicChecked;

        public bool ChkbxIsZeroDayMusicChecked
        {
            get { return _chkbxIsZeroDayMusicChecked; }
            set
            {
                _chkbxIsZeroDayMusicChecked = value;
                CheckSelectAll();
                _renamer.IsZeroDayMusicChecked = _chkbxIsZeroDayMusicChecked;
                NotifyPropertyChanged("ChkbxIsZeroDayMusicChecked");
            }
        }

        private bool _chkbxIsNewChecked;

        public bool ChkbxIsNewChecked
        {
            get { return _chkbxIsNewChecked; }
            set
            {
                _chkbxIsNewChecked = value;
                CheckSelectAll();
                _renamer.IsNewChecked = _chkbxIsNewChecked;
                NotifyPropertyChanged("ChkbxIsNewChecked");
            }
        }

        private bool _chkbxIsUkTopFortyChecked;

        public bool ChkbxIsUkTopFortyChecked
        {
            get { return _chkbxIsUkTopFortyChecked; }
            set
            {
                _chkbxIsUkTopFortyChecked = value;
                CheckSelectAll();
                _renamer.IsUkTopFortyChecked = _chkbxIsUkTopFortyChecked;
                NotifyPropertyChanged("ChkbxIsUkTopFortyChecked");
            }
        }

        private bool _chkbxIsClearCommentChecked;

        public bool ChkbxIsClearCommentChecked
        {
            get { return _chkbxIsClearCommentChecked; }
            set
            {
                _chkbxIsClearCommentChecked = value;
                CheckSelectAll();
                _renamer.IsClearCommentChecked = _chkbxIsClearCommentChecked;
                NotifyPropertyChanged("ChkbxIsClearCommentChecked");
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
                NotifyPropertyChanged("ChkbxIsUnderscoreChecked");
            }
        }

        private bool _chkbxIsReleaseNameChecked;

        public bool ChkbxIsReleaseNameChecked
        {
            get { return _chkbxIsReleaseNameChecked; }
            set
            {
                _chkbxIsReleaseNameChecked = value;
                CheckSelectAll();
                _renamer.IsReleaseNameChecked = _chkbxIsReleaseNameChecked;
                NotifyPropertyChanged("ChkbxIsReleaseNameChecked");
            }
        }

        private bool _chkbxIsFunkymixChecked;

        public bool ChkbxIsFunkymixChecked
        {
            get { return _chkbxIsFunkymixChecked; }
            set
            {
                _chkbxIsFunkymixChecked = value;
                CheckSelectAll();
                _renamer.IsFunkymixChecked = _chkbxIsFunkymixChecked;
                NotifyPropertyChanged("ChkbxIsFunkymixChecked");
            }
        }

        private bool _chkbxIsBpmAtEndChecked;

        public bool ChkbxIsBpmAtEndChecked
        {
            get { return _chkbxIsBpmAtEndChecked; }
            set
            {
                _chkbxIsBpmAtEndChecked = value;
                CheckSelectAll();
                _renamer.IsBpmAtEndChecked = _chkbxIsBpmAtEndChecked;
                NotifyPropertyChanged("ChkbxIsBpmAtEndChecked");
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
                NotifyPropertyChanged("ChkbxIsSelectAllChecked");
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
                ChkbxIsZeroDayMusicChecked = _chkbxIsSelectAllChecked;
                ChkbxIsNewChecked = _chkbxIsSelectAllChecked;
                ChkbxIsUkTopFortyChecked = _chkbxIsSelectAllChecked;
                ChkbxIsUnderscoreChecked = _chkbxIsSelectAllChecked;
                ChkbxIsClearCommentChecked = _chkbxIsSelectAllChecked;
                ChkbxIsReleaseNameChecked = _chkbxIsSelectAllChecked;
                ChkbxIsFunkymixChecked = _chkbxIsSelectAllChecked;
                ChkbxIsBpmAtEndChecked = _chkbxIsSelectAllChecked;
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
                    _chkbxIsZeroDayMusicChecked &&
                    _chkbxIsNewChecked &&
                    _chkbxIsUkTopFortyChecked &&
                    _chkbxIsClearCommentChecked &&
                    _chkbxIsReleaseNameChecked &&
                    _chkbxIsFunkymixChecked &&
                    _chkbxIsBpmAtEndChecked &&
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
            MainProgressIsIndeterminate = true;
            var patterns = _renamer.GetSelectedPatterns();

            if (IsChangeRequried(patterns))
            {
                var songs = _renamer.GetSongs();
                MainProgressIsIndeterminate = false;
                MainProgressMax = songs.Count();

                foreach (var song in songs)
                {
                    if (patterns.Any())
                    {
                        song.Id3Tag.ReplacePatterns(patterns);
                        song.ReplacePatterns(patterns);
                    }

                    song.Id3Tag.EmptyAlbum();
                    song.Id3Tag.EmptyAlbumArtist();
                    song.Id3Tag.EmptyFrames();

                    song.ReplaceHyphen();

                    if (_renamer.IsUnderscoreChecked)
                    {
                        song.Id3Tag.ReplaceUnderscores();

                        song.ReplaceUnderscores();
                    }

                    if (_renamer.IsClearCommentChecked)
                    {
                        song.Id3Tag.EmptyComment();
                    }

                    if (!string.IsNullOrEmpty(_renamer.ReleaseName))
                    {
                        song.Id3Tag.AddAlbum(_renamer.ReleaseName);
                    }

                    if (_renamer.IsReleaseNameChecked && !string.IsNullOrEmpty(_renamer.ReleaseName))
                    {
                        song.AddReleaseName(_renamer.ReleaseName);
                    }

                    song.Id3Tag.Save();
                    song.Id3Tag.Dispose();
                    MainProgressValue++;
                }
            }

            MainProgressIsIndeterminate = false;
            MessageBox.Show("Finished renaming.");
            ResetMainProgress();
        }

        private bool IsChangeRequried(List<String> patterns)
        {
            return (patterns.Any() 
                || _renamer.IsUnderscoreChecked 
                || _renamer.IsClearCommentChecked 
                || !string.IsNullOrEmpty(_renamer.ReleaseName));
        }

        private void ResetMainProgress()
        {
            MainProgressValue = 0;
            MainProgressMax = 1;
        }

        public void Consolidate()
        {
            _renamer.Consolidate();
            MessageBox.Show("Finished consolidation.");
        }
    }
}
