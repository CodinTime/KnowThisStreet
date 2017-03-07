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
    public partial class Game : Form
    {
        Random random;
        Street street;
        List<Street> streetList;
        bool isStarted = false;
        int actualStreetNr;
        public Game()
        {
            InitializeComponent();
            ReadData();

            Map.map = new Bitmap(Map.imageName);
            pictureBox1.Image = Map.map;
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

        private void Game_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            random = new Random();
            streetList = Map.streets.ToList();

            if (streetList.Count < 1)
                return;

            actualStreetNr = random.Next(streetList.Count);
            street = streetList[actualStreetNr];
            streetList.RemoveAt(actualStreetNr);
            labelStreet.Text = street.Name;
            isStarted = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (streetList == null || !isStarted)
                return;
            if (streetList.Count == 0)
                isStarted = false;

            MouseEventArgs me = (MouseEventArgs)e;
            int x = me.X;
            int y = me.Y;

            double d = -1;

            for (int i = 0; i < street.points.Count - 1; i++)
            {
                double x1 = street.points[i].X;
                double y1 = street.points[i].Y;
                double x2 = street.points[i + 1].X;
                double y2 = street.points[i + 1].Y;
                double tmp = 0.0;
                
                if (x1 == x2)
                    tmp = Math.Abs(x - x1);
                else
                    tmp = Math.Abs((y2 - y1) / (x2 - x1) * x - y + (x2 * y1 - x1 * y2) / (x2 - x1)) / Math.Sqrt(Math.Pow((y2 - y1) / (x2 - x1), 2) + 1);
                               

                if (d < 0 || tmp < d)
                    d = tmp;
            }
            

            labelDistance.Text = d.ToString();
            if (d < 10)
            {
                DrawStreet(actualStreetNr, true);
                labelDistance.ForeColor = Color.LightGreen;
            }
            else
            {
                DrawStreet(actualStreetNr, false);
                labelDistance.ForeColor = Color.Red;
            }

            actualStreetNr = random.Next(streetList.Count);
            if (streetList.Count > 0)
            {
                street = streetList[actualStreetNr];
                streetList.RemoveAt(actualStreetNr);
            }

            labelStreet.Text = street.Name;
        }
        public void DrawStreet(int streetId, bool isCorrect)
        {
            Pen pen = null;
            if (isCorrect)
                pen = new Pen(Color.LightGreen, 5);
            else
                pen = new Pen(Color.Red, 5);

            Map.map = new Bitmap(Map.imageName);
            using (Graphics g = Graphics.FromImage(Map.map))
            {
                for (int i = 0; i < street.points.Count - 1; i++)
                {
                    g.DrawLine(pen, street.points[i].X, street.points[i].Y, street.points[i + 1].X, street.points[i + 1].Y);
                }

            }
            pictureBox1.Image = Map.map;
        }
    }
}
