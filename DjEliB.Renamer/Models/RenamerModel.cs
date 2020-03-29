using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public bool IsZeroDayMusicChecked { get; set; }
        public bool IsNewChecked { get; set; }
        public bool IsUkTopFortyChecked { get; set; }
        public bool IsClearCommentChecked { get; set; }
        public bool IsUnderscoreChecked { get; set; }
        public string ReleaseName { get; set; }
        public bool IsReleaseNameChecked { get; set; }
        public bool IsFunkymixChecked { get; set; }
        public bool IsBpmAtEndChecked { get; set; }
        public bool IsTransitionChecked { get; set; }

        public RenamerModel()
        {
            SourceDirectory = @"C:\DJ Playlists\Unprocessed\To be processed";
        }

        public void Consolidate()
        {
            if (Directory.Exists(SourceDirectory))
            {
                SetAttributesNormal(new DirectoryInfo(SourceDirectory));

                ConsolidateSubdirectories(SourceDirectory);
                DeleteSubdirectories(SourceDirectory);
            }
        }

        public void DeleteSubdirectories(string targetDirectory)
        {
            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);

            foreach (var subdirectory in subdirectoryEntries)
            {
                DeleteSubdirectories(subdirectory);
                Directory.Delete(subdirectory);
            }
        }

        public void SetAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
            {
                SetAttributesNormal(subDir);
            }

            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }
        }

        public void ConsolidateSubdirectories(string targetDirectory)
        {
            if (targetDirectory != SourceDirectory)
            {
                var fileEntries = Directory.GetFiles(targetDirectory);

                foreach (var fileName in fileEntries)
                {
                    MoveFile(fileName);
                }
            }

            var subdirectoryEntries = Directory.GetDirectories(targetDirectory);

            foreach (var subdirectory in subdirectoryEntries)
            {
                ConsolidateSubdirectories(subdirectory);
            }
        }

        public void MoveFile(string path)
        {
            if (File.Exists(path))
            {
                if (IsMusicFile(path))
                {
                    var fileName = Path.GetFileName(path);
                    var sourcePath = string.Concat(SourceDirectory, @"\", fileName);
                    var isThisFileLargest = true;

                    if (File.Exists(sourcePath))
                    {
                        isThisFileLargest = Song.GetFileLength(path) >= Song.GetFileLength(sourcePath);

                        // Delete smallest file
                        var smallestFilePath = isThisFileLargest ? sourcePath : path;
                        File.SetAttributes(smallestFilePath, FileAttributes.Normal);
                        File.Delete(smallestFilePath);
                    }

                    if (isThisFileLargest)
                    {
                        File.Move(path, sourcePath);
                    }
                }
                else
                {
                    File.Delete(path);
                }
            }
        }

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

        public List<Song> GetSongs()
        {
            var songs = new List<Song>();
            var files = Directory.GetFiles(SourceDirectory);

            foreach (string path in files)
            {
                if (IsMusicFile(path))
                {
                    songs.Add(new Song(path));
                }
            }

            return songs;
        }

        public bool IsMusicFile(string path)
        {
            return Regex.IsMatch(path, Song.SupportedExtensionPattern, RegexOptions.IgnoreCase);
        }

        public List<string> GetSelectedPatterns()
        {
            var patterns = new List<string>();

            if (IsSinglesChecked)
            {
                patterns.Add(Song.SinglesPattern);
            }

            if (IsElectroHouseChecked)
            {
                patterns.Add(Song.ElectroHousePattern);
            }

            if (IsDjFtpChecked)
            {
                patterns.Add(Song.FtpPattern);
            }

            if (IsZeroDayMusicChecked)
            {
                patterns.Add(Song.ZeroDayMusicPattern);
            }

            if (IsNewChecked)
            {
                patterns.Add(Song.NewPattern);
            }

            if (IsUkTopFortyChecked)
            {
                patterns.Add(Song.UkTopFortyPattern);
            }

            if (IsFunkymixChecked)
            {
                patterns.Add(Song.FunkymixPattern);
            }

            if (IsBpmAtEndChecked)
            {
                patterns.Add(Song.BpmAtEndPattern);
            }

            return patterns;
        }

        public string GetTransitionPattern()
        {
            var pattern = string.Empty;

            if (IsTransitionChecked)
            {
                pattern = Song.TransitionPattern;
            }

            return pattern;
        }
    }
}
