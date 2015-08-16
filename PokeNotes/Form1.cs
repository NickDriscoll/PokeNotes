using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PokeNotes
{
    public partial class Form1 : Form
    {

        string gender = "Male";
        string hpIv = "";
        string atkIv = "";
        string defIv = "";
        string spAtkIv = "";
        string spDefIv = "";
        string speedIv = "";

        public Form1()
        {
            InitializeComponent();
        }

        public string checkBoxTest(bool check)
        {
            if (check)
            {
                return "x";
            }
            else
            {
                return "";
            }
        }

        /*private byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }*/

        private byte[] ObjectToByteArray(Object obj)
        {
            if (obj == null)
            {
                return null;
            }
            else
            {
                BinaryFormatter bf = new BinaryFormatter();
                byte[] buffer = new byte[1024];
                MemoryStream ms = new MemoryStream(buffer, 0, buffer.Length, true, true);
                bf.Serialize(ms, obj);
                return ms.GetBuffer();
            }
        }

        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream(arrBytes);
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
        
        [Serializable()]
        class Pokemon
        {
            public string Name { get; set; }
            public string Item { get; set; }
            public string Gender { get; set; }
            public string HPIV { get; set; }
            public string AtkIV { get; set; }
            public string DefIV { get; set; }
            public string SpAtkIV { get; set; }
            public string SpDefIV { get; set; }
            public string SpeedIV { get; set; }
        }

        List<byte[]> data = new List<byte[]>();
        private void TestButton_Click(object sender, EventArgs e)
        {
            haveSaved = false;

            byte[] saveablePokemon = new byte[1024];
            try
            {
                //Generates the Pokemon object
                Pokemon pokemon = new Pokemon();
                pokemon.Name = textBox1.Text;
                pokemon.Gender = gender;
                pokemon.Item = comboBox1.Text;
                pokemon.HPIV = hpIv;
                pokemon.AtkIV = atkIv;
                pokemon.DefIV = defIv;
                pokemon.SpAtkIV = spAtkIv;
                pokemon.SpDefIV = spDefIv;
                pokemon.SpeedIV = speedIv;

                //Attempts to write current pokemon to data
                saveablePokemon = ObjectToByteArray(pokemon);
                data.Add(saveablePokemon);

                //Handles pushing data to the table
                ListViewItem lvi = new ListViewItem(pokemon.Name);
                lvi.SubItems.Add(pokemon.Gender);
                lvi.SubItems.Add(pokemon.Item);
                lvi.SubItems.Add(pokemon.HPIV);
                lvi.SubItems.Add(pokemon.AtkIV);
                lvi.SubItems.Add(pokemon.DefIV);
                lvi.SubItems.Add(pokemon.SpAtkIV);
                lvi.SubItems.Add(pokemon.SpDefIV);
                lvi.SubItems.Add(pokemon.SpeedIV);
                listView1.Items.Add(lvi);                
            }
            catch
            {
                MessageBox.Show("The name entered is too long. Please enter a shorter name.");
            }                        
        }

        bool haveSaved = true;
        string activeFile;
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            haveSaved = true;
            //instantiates an instance of SaveFileDialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "pokenote";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Get file name and make it the active file.
                string name = saveFileDialog.FileName;
                activeFile = name;
                // Write to the file name selected.
                FileStream fs = new FileStream(name, FileMode.OpenOrCreate);
                for (int i = 0; i < data.Count; i++)
                {
                    fs.Write(data[i], 0, data[i].Length);
                }
                fs.Flush();
                fs.Close();
            }
        }

        public void loadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Cleaning up current session
                listView1.Items.Clear();
                data.Clear();

                //Get file name and make it the active file.
                string name = openFileDialog.FileName;
                activeFile = name;
                //Read selected file name and write objects to list
                //NOTE: One pokemon takes up 100 bytes.
                FileStream fs = new FileStream(name, FileMode.Open);
                int numberOfPokemon = (int)fs.Length / 1024;
                for (int i = 0; i <= numberOfPokemon - 1; i++)
                {
                    fs.Read(buffer, 0, buffer.Length);
                    Pokemon pokemon = (Pokemon)ByteArrayToObject(buffer);

                    //Handles pushing data to the table

                    ListViewItem lvi = new ListViewItem(pokemon.Name);
                    lvi.SubItems.Add(pokemon.Gender);
                    lvi.SubItems.Add(pokemon.Item);
                    lvi.SubItems.Add(pokemon.HPIV);
                    lvi.SubItems.Add(pokemon.AtkIV);
                    lvi.SubItems.Add(pokemon.DefIV);
                    lvi.SubItems.Add(pokemon.SpAtkIV);
                    lvi.SubItems.Add(pokemon.SpDefIV);
                    lvi.SubItems.Add(pokemon.SpeedIV);
                    listView1.Items.Add(lvi);
                    byte[] saveablePokemon = ObjectToByteArray(pokemon);
                    data.Add(saveablePokemon);
                }
                fs.Flush();
                fs.Close();
            }
        }

        byte[] buffer = new byte[1024];
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveSaved == false)
            {
                var confirmResult = MessageBox.Show("You haven't saved recently. Are you sure you want to continue?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    loadFile();
                    haveSaved = true;
                }
            }
            else
            {
                loadFile();
            }            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            hpIv = checkBoxTest(checkBox1.Checked);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            atkIv = checkBoxTest(checkBox4.Checked);
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            spAtkIv = checkBoxTest(checkBox6.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            speedIv = checkBoxTest(checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            defIv = checkBoxTest(checkBox3.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            spDefIv = checkBoxTest(checkBox5.Checked);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            gender = "Female";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            gender = "Male";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            haveSaved = false;
            gender = "N/A";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (haveSaved == false)
            {
                var confirmResult = MessageBox.Show("You haven't saved recently. Are you sure you want to continue?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    listView1.Items.Clear();
                    data.Clear();
                    haveSaved = true;
                }
            }
            else
            {
                listView1.Items.Clear();
                data.Clear();
            }            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                foreach (ListViewItem eachItem in listView1.SelectedItems)
                {
                    //TODO: Add code that will remove the pokemon from 'data' as well.
                    listView1.Items.Remove(eachItem);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (haveSaved == false)
            {
                var confirmResult = MessageBox.Show("You haven't saved recently. Are you sure you want to continue?", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            haveSaved = true;
            // Write to the file name selected.
            if (activeFile != null)
            {
                FileStream fs = new FileStream(activeFile, FileMode.OpenOrCreate);
                for (int i = 0; i < data.Count; i++)
                {
                    fs.Write(data[i], 0, data[i].Length);
                }
                fs.Flush();
                fs.Close();
            }
            else
            {
                saveToolStripMenuItem_Click(sender, e);
            }
        }
    }
}
