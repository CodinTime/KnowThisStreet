using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KnowThisStreet
{
    public partial class Form1 : Form
    {
        string[] names;
        public Form1()
        {
            InitializeComponent();

            refreshFiles();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Forms.EditMap window = new Forms.EditMap();
            window.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Forms.Game window = new Forms.Game();
            window.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Map.fileName = names[comboBox1.SelectedIndex];
            XmlReader xr = XmlReader.Create(Map.fileName);
            while (xr.Read())
            {
                if (xr.Name == "streets")
                {
                    if (xr.NodeType == XmlNodeType.Element)
                    {
                        pictureBoxThumbnail.Image = new Bitmap(xr.GetAttribute("image"));
                        string count = xr.GetAttribute("count");
                        toolStripStatusLabelStreetCount.Text = "Number of streets: " + count;
                        break;
                    }
                }
            }
            xr.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            comboBox1.Focus();
        }

        private void buttonAddMap_Click(object sender, EventArgs e)
        {
            openFileDialogMap.Filter = "obraz jpg|*.jpg";
            openFileDialogMap.ShowDialog();
        }

        private void openFileDialogMap_FileOk(object sender, CancelEventArgs e)
        {
            int i = 1;
            string fileName = openFileDialogMap.FileName;
            string mapFileName = "";
            string stateFileName = "";
            using (Bitmap b = new Bitmap(fileName))
            {

                while (File.Exists(Map.addMapDirectory(@"map") + i + ".jpg") || File.Exists(Map.addMapDirectory(@"state") + i + ".xml")) i++;

                mapFileName = Map.addMapDirectory("map") + i + ".jpg";
                stateFileName = Map.addMapDirectory("state") + i + ".xml";
                b.Save(mapFileName);
            }

            XmlWriter xmlWriter = XmlWriter.Create(stateFileName);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("streets");
            xmlWriter.WriteAttributeString("image", mapFileName);
            xmlWriter.WriteAttributeString("count", "0");

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            refreshFiles();

        }

        private void refreshFiles()
        {
            names = Directory.GetFiles(@".\maps\", "*.xml");
            foreach (var n in names)
            {
                string name = Path.GetFileNameWithoutExtension(n);
                comboBox1.Items.Add(name);
            }
            if(names.Length > 0)
                comboBox1.SelectedIndex = 0;
        }
        
    }
}
