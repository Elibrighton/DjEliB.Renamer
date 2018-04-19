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

        public void RemovePatterns(List<string> patterns)
        {
            foreach (var pattern in patterns)
            {
                UpdateTitle(pattern, _tagLibFile.Tag.Title);
                UpdateAlbumArtists(pattern, _tagLibFile.Tag.FirstAlbumArtist);
                UpdateAlbum(pattern, _tagLibFile.Tag.Album);
                UpdateComment();
            }

            Save();
        }

        public void UpdateTitle(string pattern, string title = null)
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Title = Song.ReplacePattern(pattern, title);
            }
        }

        public void UpdateAlbumArtists(string pattern, string artist = null)
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.AlbumArtists = new[] { Song.ReplacePattern(pattern, artist) };
            }
        }

        public void UpdateAlbum(string pattern, string album = null)
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Album = Song.ReplacePattern(pattern, album);
            }
        }

        public void UpdateComment(string comment = null)
        {
            if (_tagLibFile != null)
            {
                _tagLibFile.Tag.Comment = comment;
            }
        }
    }
}
