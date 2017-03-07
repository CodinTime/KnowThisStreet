using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KnowThisStreet.Forms
{
    public partial class EditMap : Form
    {
        Street street;
        public EditMap()
        {
            InitializeComponent();
            ReadData();

            Map.map = new Bitmap(Map.imageName);
            pictureBox1.Image = Map.map;
            foreach (var street in Map.streets)
            {
                listBox2.Items.Add(street);
            }
            
        }

        public void ReadData()
        {
            Map.streets = new List<Street>();
            XmlReader xr;
            xr = XmlReader.Create(Map.fileName);
            int x, y;
            while (xr.Read())
            {
                Console.WriteLine(xr.Name);
                if (xr.Name == "streets")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                        Map.imageName = xr.GetAttribute("image");
                }
                if (xr.Name == "street")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                        street = new Street(xr.GetAttribute("name"));
                    else if (xr.NodeType == XmlNodeType.EndElement)
                        Map.streets.Add(street);
                }
                else if (xr.Name == "point")
                {
                    x = Int32.Parse(xr.GetAttribute("x"));
                    y = Int32.Parse(xr.GetAttribute("y"));
                    street.points.Add(new Point(x, y));
                }

            }
            xr.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (street == null)
                return;

            MouseEventArgs me = (MouseEventArgs)e;
            Pen pen = new Pen(Color.Red, 3);

            listBox1.Items.Add(me.Location);
            street.AddPoint(me.Location);

            using (Graphics g = Graphics.FromImage(Map.map))
            {
                if (street.points.Count == 1)
                    g.DrawEllipse(pen, street.points[0].X - 1, street.points[0].Y - 1, 2, 2);
                if (street.points.Count > 1)
                    g.DrawLine(pen, street.points[street.points.Count - 2].X, street.points[street.points.Count - 2].Y, me.Location.X, me.Location.Y);
            }
            pictureBox1.Image = Map.map;
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Map.map = new Bitmap(Map.imageName);
            pictureBox1.Image = Map.map;
            listBox1.Items.Clear();
            street = new Street(textBox1.Text);
            Map.streets.Add(street);
            listBox2.Items.Clear();
            foreach (var s in Map.streets)
            {
                listBox2.Items.Add(s);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0)
                return;
            Pen pen = new Pen(Color.Red, 3);
            street = Map.streets[listBox2.SelectedIndex];
            listBox1.Items.Clear();
            foreach (var p in street.points)
            {
                listBox1.Items.Add(p);
            }
            Map.map = new Bitmap(Map.imageName);
            using (Graphics g = Graphics.FromImage(Map.map))
            {
                for (int i = 0;i  < street.points.Count-1; i++)
                {
                    g.DrawLine(pen, street.points[i].X, street.points[i].Y, street.points[i+1].X, street.points[i+1].Y);
                }                

            }
            pictureBox1.Image = Map.map;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex < 0)
                return;
            Map.streets.RemoveAt(listBox2.SelectedIndex);
            listBox2.Items.RemoveAt(listBox2.SelectedIndex);
            Map.map = new Bitmap(Map.imageName);
            pictureBox1.Image = Map.map;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            XmlWriter xmlWriter = XmlWriter.Create(Map.fileName);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("streets");
            xmlWriter.WriteAttributeString("image", Map.imageName);
            xmlWriter.WriteAttributeString("count", Map.streets.Count.ToString());

            foreach (var s in Map.streets)
            {
                xmlWriter.WriteStartElement("street");
                xmlWriter.WriteAttributeString("name", s.Name);                

                foreach (var p in s.points)
                {
                    xmlWriter.WriteStartElement("point");
                    xmlWriter.WriteAttributeString("x", p.X.ToString());
                    xmlWriter.WriteAttributeString("y", p.Y.ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

            }           
            
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            Map.streets = Map.streets.OrderBy(o => o.Name).ToList();
            listBox2.Items.Clear();
            foreach (var s in Map.streets)
            {
                listBox2.Items.Add(s);
            }
        }
    }
}
