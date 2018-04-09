using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DjEliB.Renamer
{
    public class Song
    {
        public Song (string fileName)
        {
            FileName = fileName;
        }
        
        public string FileName { get; set; }
        public bool IsNumberedArtist { get; set; } // use expression body to set the prop
    }
}
