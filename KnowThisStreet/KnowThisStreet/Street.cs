using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowThisStreet
{
    public class Street
    {        
        public List<Point> points { get; set; }
        public string Name { get; set; }

        public Street(string name)
        {
            points = new List<Point>();
            Name = name;         
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
