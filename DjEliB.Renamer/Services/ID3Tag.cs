using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DjEliB.Renamer.Services
{
    public class ID3Tag
    {
        private TagLib.File _tagLibFile;

        public ID3Tag(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("The path is invalid.");
            }

            _tagLibFile = TagLib.File.Create(path);
        }

        public void Dispose()
        {
            _tagLibFile.Dispose();
        }

        public void Save()
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Save();
            }
        }

        public void ReplacePatterns(List<string> patterns)
        {
            foreach (var pattern in patterns)
            {
                if (_tagLibFile != null)
                {
                    var replacementText = pattern == Song.FunkymixPattern ? "Funkymix" : "";

                    _tagLibFile.Tag.Title = Song.ReplacePattern(pattern, _tagLibFile.Tag.Title, replacementText);

                    var renamedPerformer = Song.ReplacePattern(pattern, _tagLibFile.Tag.FirstPerformer, replacementText);
                    _tagLibFile.Tag.Performers = null;
                    _tagLibFile.Tag.Performers = new[] { renamedPerformer };

                    _tagLibFile.Tag.Lyrics = Song.ReplacePattern(pattern, _tagLibFile.Tag.Lyrics, replacementText);
                }
            }
        }

        internal void EmptyComment()
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Comment = string.Empty;
            }
        }

        internal void PreserveEnergyInComment()
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Comment = Song.ExtractPattern(@"Energy\s(10$|\d$)", _tagLibFile.Tag.Comment);
            }
        }

        internal void EmptyAlbum()
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Album = string.Empty;
            }
        }

        internal void EmptyAlbumArtist()
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.AlbumArtists = null;
                _tagLibFile.Tag.AlbumArtists = new[] { "" };
            }
        }

        internal void AddAlbum(string album)
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Album = album;
            }
        }

        internal void EmptyFrames()
        {
            if (_tagLibFile != null)
            {
                var id3v2Tag = (TagLib.Id3v2.Tag)_tagLibFile.GetTag(TagLib.TagTypes.Id3v2);

                if (id3v2Tag != null)
                {
                    // reference {https://en.wikipedia.org/wiki/ID3#ID3v2_Frame_Specification_.28Version_2.3.29}
                    id3v2Tag.SetTextFrame("WOAF", "");
                    id3v2Tag.SetTextFrame("TRSN", "");
                    id3v2Tag.SetTextFrame("TXXX", "");
                    id3v2Tag.SetTextFrame("WXXX", "");
                    id3v2Tag.SetTextFrame("TENC", "");
                }
            }
        }

        internal void ReplaceUnderscores()
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Title = ReplaceUnderscore(_tagLibFile.Tag.Title);

                var replacedPerformer = ReplaceUnderscore(_tagLibFile.Tag.FirstPerformer);
                _tagLibFile.Tag.Performers = null;
                _tagLibFile.Tag.Performers = new[] { replacedPerformer };
            }
        }

        private string ReplaceUnderscore(string songText)
        {
            if (!string.IsNullOrEmpty(songText))
            {
                if (!songText.Contains(" ") && songText.Contains("_"))
                {
                    songText = songText.Replace("_", " ");
                    songText.Trim();
                }
            }

            return songText;
        }
    }
}
