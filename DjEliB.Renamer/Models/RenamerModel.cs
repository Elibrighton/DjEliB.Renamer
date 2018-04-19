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
        public bool IsUnderscoreChecked { get; set; }

        private const string singlesPattern = @"^[0-9]{2}[0-9]?(\.|\.\s|\s?\-\s?|\s)";
        private const string electroHousePattern = @"(\s?\-?\s?|http\:\\\\)ElectroHouse\.ucoz\.com";
        private const string ftpPattern = @"DJFTP\.COM";
        private const string supportedExtensionPattern = @"\.(mp3$|wav$|mp4$)";

        public RenamerModel()
        {
            SourceDirectory = @"C:\DJ Eli B\Unprocessed";
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

        public void Rename()
        {
            var patterns = GetSelectedPatterns();

            if (patterns.Any())
            {
                var songs = GetSongs();

                foreach (var song in songs)
                {
                    // Update ID3Tags
                    song.Id3Tag.RemovePatterns(patterns);
                    song.Id3Tag.Dispose();

                    song.RemovePatterns(patterns);
                }
            }
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

            // where is underscore pattern?

            return patterns;
        }
    }
}
