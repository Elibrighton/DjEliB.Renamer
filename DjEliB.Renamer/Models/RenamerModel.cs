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
            var selectedPatterns = GetSelectedPatterns();
            var songs = GetSongs();

            foreach (var song in songs)
            {
                if (IsSinglesChecked)
                {
                    if (!song.IsNumberedArtist)
                    {

                    }
                }
            }
        }

        public List<Song> GetSongs()
        {
            var songs = new List<Song>();
            var fileEntries = Directory.GetFiles(SourceDirectory);

            foreach (string fileName in fileEntries)
            {
                if (IsMusicFile(fileName))
                {
                    songs.Add(new Song(fileName));
                }
            }

            return songs;
        }

        public bool IsMusicFile(string fileName)
        {
            return Regex.IsMatch(fileName, @"\.(mp3$|wav$|mp4$)", RegexOptions.IgnoreCase);
        }

        public List<string> GetSelectedPatterns()
        {
            var patterns = new List<string>();

            if (IsSinglesChecked)
            {
                patterns.Add(@"^[0-9]{2}[0-9]?(\.|\.\s|\s?\-\s?|\s)");
            }

            if (IsElectroHouseChecked)
            {
                patterns.Add(@"(\s?\-?\s?|http\:\\\\)ElectroHouse\.ucoz\.com");
            }

            if (IsDjFtpChecked)
            {
                patterns.Add(@"DJFTP\.COM");
            }

            return patterns;
        }
    }
}
