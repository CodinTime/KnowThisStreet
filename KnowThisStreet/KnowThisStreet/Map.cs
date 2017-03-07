using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KnowThisStreet
{
    public static class Map
    {        
        public static List<Street> streets { get; set; }
        public static string fileName { get; set; } = @".\maps\state1.xml";
        public static string imageName { get; set; } = @"maps\map1.jpg";
        public static Bitmap map = new Bitmap(imageName);
        //public  XmlDocument state = new XmlDocument();

        public static string addMapDirectory(string filename)
        {
            return "maps/" + filename;
        }
    }
}
