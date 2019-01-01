using DjEliB.Renamer.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DjEliB.Renamer
{
    public class Song
    {
        public const string SinglesPattern = @"^\.?[0-9]+(\.|\.\s|\s?\-\s?|\s)";
        public const string ElectroHousePattern = @"(\s?\-?\s?|http\:\\\\)ElectroHouse\.ucoz\.com";
        public const string FtpPattern = @"DJFTP\.COM";
        public const string ZeroDayMusicPattern = @"www\.0daymusic\.org";
        public const string NewPattern = @"\(new\)";
        public const string UkTopFortyPattern = @"\-\sUK\sTop\s40\s\[\d\d\-\d\d\-\d\d\d\d\]\s\-\s\[\d\d\d\]";
        public const string FunkymixPattern = @"Funkymiix";
        public const string BpmAtEndPattern = @"\d\d\d?$";
        public const string SupportedExtensionPattern = @"\.(mp3$|wav$|mp4$)";

        public Song(string path)
        {
            Path = path;
            Id3Tag = new ID3Tag(Path);
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("The path is invalid.");
                }

                _path = value;

                Id3Tag = new ID3Tag(_path);
                Directory = System.IO.Path.GetDirectoryName(_path);
                FileName = System.IO.Path.GetFileNameWithoutExtension(_path);
                Extension = System.IO.Path.GetExtension(_path);
            }
        }

        public string Directory { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public ID3Tag Id3Tag { get; set; }

        private string _path;

        public void ReplacePatterns(List<string> patterns)
        {
            foreach (var pattern in patterns)
            {
                var replacementText = pattern == Song.FunkymixPattern ? "Funkymix" : "";

                if (!IsNumberedArtist(FileName) || pattern != SinglesPattern)
                {
                    var renamedFileName = ReplacePattern(pattern, FileName, replacementText);
                    RenameFile(renamedFileName);
                }
            }
        }

        public void RenameFile(string renamedFileName)
        {
            var renamedPath = string.Concat(Directory, @"\", renamedFileName, Extension);

            if (renamedPath != _path)
            {
                var isThisFileLargest = true;
                var isFileRenamed = false;

                if (File.Exists(renamedPath))
                {
                    isThisFileLargest = GetFileLength(_path) >= GetFileLength(renamedPath);

                    // Delete smallest file
                    File.Delete(isThisFileLargest ? renamedPath : _path);

                    if (!isThisFileLargest)
                    {
                        isFileRenamed = true;
                    }
                }

                if (isThisFileLargest)
                {
                    File.Move(_path, renamedPath);
                    isFileRenamed = File.Exists(renamedPath);
                }

                if (isFileRenamed)
                {
                    Path = renamedPath;
                }
            }
        }

        public static long GetFileLength(string path)
        {
            return new FileInfo(path).Length;
        }

        public static bool IsNumberedArtist(string songText)
        {
            var isNumberedArtist = false;

            var numberedArtists = GetNumberedArtists();

            if (!string.IsNullOrEmpty(songText))
            {
                foreach (string numberedArtist in numberedArtists)
                {
                    if (Regex.IsMatch(songText, numberedArtist, RegexOptions.IgnoreCase))
                    {
                        isNumberedArtist = true;
                        break;
                    }
                }
            }

            return isNumberedArtist;
        }

        public static string ReplacePattern(string pattern, string songText, string replacementText = "")
        {
            if (!string.IsNullOrEmpty(songText))
            {
                if (!Song.IsNumberedArtist(songText) || pattern != SinglesPattern)
                {
                    if (Regex.IsMatch(songText, pattern))
                    {
                        songText = Regex.Replace(songText, pattern, replacementText);
                        songText = songText.Trim();
                    }
                }
            }

            return songText;
        }

        public static string ExtractPattern(string pattern, string songText)
        {
            if (!string.IsNullOrEmpty(songText))
            {
                if (Regex.IsMatch(songText, pattern))
                {
                    var match = Regex.Match(songText, pattern);
                    songText = match.Value.Trim();
                }
            }

            return songText;
        }

        internal void ReplaceUnderscores()
        {
            if (!FileName.Contains(" ") && FileName.Contains("_"))
            {
                var renamedFileName = FileName.Replace("_", " ");
                RenameFile(renamedFileName);
            }
        }

        internal void ReplaceHyphen()
        {
            if (FileName.Contains("–"))
            {
                var renamedFileName = FileName.Replace("–", "-");
                RenameFile(renamedFileName);
            }
        }

        internal void AddReleaseName(string releaseName)
        {
            releaseName = releaseName.Replace("(", "").Replace(")", "");
            var pattern = string.Concat(@"\(", releaseName, @".*\)");

            if (!Regex.IsMatch(FileName, pattern))
            {
                releaseName = string.Concat("(", releaseName, ")");
                var renamedFileName = string.Concat(FileName, " ", releaseName);
                RenameFile(renamedFileName);
            }
        }

        private static string[] GetNumberedArtists()
        {
            string[] numberedArtist = {
                                                        @"^0\sT0\s100\sThe",
                                                        @"^1,\s2\sStep",
                                                        @"^100it",
                                                        @"^106\sMiles",
                                                        @"^112\s-\sDANCE",
                                                        @"^112\sFt.",
                                                        @"^112\s-\sHot",
                                                        @"^112\s-\sOnly",
                                                        @"^112\s-\sPeaches",
                                                        @"^112\sFeat.",
                                                        @"^1975",
                                                        @"^1927",
                                                        @"^123XYZ",
                                                        @"^1999",
                                                        @"^10\sDigits",
                                                        @"^10,000\sManiacs",
                                                        @"^257ers",
                                                        @"^2Pac",
                                                        @"^2Play",
                                                        @"^24hrs",
                                                        @"^2NE1",
                                                        @"^2012\s\(",
                                                        @"^211\sft.",
                                                        @"^213\s-\sSo",
                                                        @"^2\sAM",
                                                        @"^2\sFabiola",
                                                        @"^2-4\sGrooves",
                                                        @"^2Elements",
                                                        @"^2\sOn",
                                                        @"^2\sIn\sA\sRoom",
                                                        @"^2\sLive\sCrew",
                                                        @"^2\sMilly",
                                                        @"^2\sPistols",
                                                        @"^20\sfingers",
                                                        @"^21\sSavage",
                                                        @"^24K\sMagic",
                                                        @"^24\sHours",
                                                        @"^2\sPac",
                                                        @"^28\sDays",
                                                        @"^2\sChainz",
                                                        @"^2\sTimes",
                                                        @"^2\sIn\sA\sRoom",
                                                        @"^2\sUNLIMITED",
                                                        @"^2\sbrothers\son",
                                                        @"^2000\sAnd\sOne",
                                                        @"^311\s-\sAmber",
                                                        @"^360\s-",
                                                        @"^360\sFt\.\s",
                                                        @"^3T",
                                                        @"^3LAU",
                                                        @"^3BOL",
                                                        @"^3rd",
                                                        @"^3LW",
                                                        @"^3Oh3",
                                                        @"^3Oh!3",
                                                        @"^33Hz",
                                                        @"^30\sSeconds\sTo\sMars",
                                                        @"^3\sDoors\sDown",
                                                        @"^3\sThes\sHard",
                                                        @"^4B",
                                                        @"^4\s?PM",
                                                        @"^4\sNon\sBlondes",
                                                        @"^4\sThe\sCause",
                                                        @"^5\sSeconds\sOf\sSummer",
                                                        @"^5\s&\sA\sDime",
                                                        @"^50\scent",
                                                        @"^5\sO'Clock",
                                                        @"^50\sWays\sTo\sSay\sGoodbye",
                                                        @"^6\sAM",
                                                        @"^6\sInch",
                                                        @"^6lack",
                                                        @"^6\sInch",
                                                        @"^6treg",
                                                        @"^6\sBoys",
                                                        @"^666\s-\sBomba",
                                                        @"^666\s-\sSupa",
                                                        @"^6ix9ine",
                                                        @"^6lack",
                                                        @"^6ix9ine",
                                                        @"^6\sFoot\s7\sFoot",
                                                        @"^7\s11x",
                                                        @"^7\sTemperature",
                                                        @"^7\sYears",
                                                        @"^702\s-\sWhere",
                                                        @"^8ers",
                                                        @"^90's",
                                                        @"^9pm",
                                                        @"^95\sSouth",
                                                        @"^98\sDegrees",
                                                        @"^99\sproblems",
                                                        @"^99\sSouls",
                                                        @"^914\sHit\sSquad",
                                                        @"^99X"
                        };

            return numberedArtist;
        }
    }
}
