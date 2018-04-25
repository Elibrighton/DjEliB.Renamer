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
        public bool IsUnderscoreChecked { get; set; }

        private const string singlesPattern = @"^\.?[0-9]+(\.|\.\s|\s?\-\s?|\s)";
        private const string electroHousePattern = @"(\s?\-?\s?|http\:\\\\)ElectroHouse\.ucoz\.com";
        private const string ftpPattern = @"DJFTP\.COM";
        private const string zeroDayMusicPattern = @"www\.0daymusic\.org";
        private const string newPattern = @"\(new\)";
        private const string ukTopFortyPattern = @"\-\sUK\sTop\s40\s\[\d\d\-\d\d\-\d\d\d\d\]\s\-\s\[\d\d\d\]";
        private const string supportedExtensionPattern = @"\.(mp3$|wav$|mp4$)";

        public RenamerModel()
        {
            SourceDirectory = @"C:\DJ Eli B\Unprocessed";
        }

        public void Consolidate()
        {
            if (Directory.Exists(SourceDirectory))
            {
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
            return Regex.IsMatch(path, supportedExtensionPattern, RegexOptions.IgnoreCase);
        }

        public List<string> GetSelectedPatterns()
        {
            var patterns = new List<string>();

            if (IsSinglesChecked)
            {
                patterns.Add(singlesPattern);
            }

            if (IsElectroHouseChecked)
            {
                patterns.Add(electroHousePattern);
            }

            if (IsDjFtpChecked)
            {
                patterns.Add(ftpPattern);
            }

            if (IsZeroDayMusicChecked)
            {
                patterns.Add(zeroDayMusicPattern);
            }

            if (IsNewChecked)
            {
                patterns.Add(newPattern);
            }

            if (IsUkTopFortyChecked)
            {
                patterns.Add(ukTopFortyPattern);
            }

            return patterns;
        }
    }
}
