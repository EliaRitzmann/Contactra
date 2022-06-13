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
using System.Diagnostics;

namespace Contact_Book
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            txb_Path.Text = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contacts by Elia Ritzmann";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contacts by Elia Ritzmann");
        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            if (MessageBox.Show("Do you want to delete all Data?",
                                        "Delete all Data",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contacts by Elia Ritzmann", true);
                    MessageBox.Show("The files are deleted.", "Files Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show("One or more Photo-Files are currently used by another process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C:\";
            sfd.RestoreDirectory = true;
            sfd.FileName = "ContactraData.Ctd";
            sfd.DefaultExt = "txt";
            sfd.Filter = "txt files (*.txt) | *.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contacts by Elia Ritzmann\data.txt", sfd.FileName, true);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Contactra contactra = new Contactra();
            OpenFileDialog pfd = new OpenFileDialog();
            pfd.Filter = "txt files(*.txt) | *.txt";

            if (pfd.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr1 = new StreamReader(pfd.FileName);
                string line;
                var list = new List<string>();
                while ((line = sr1.ReadLine()) != null)
                {
                    list.Add(line);
                }
                //CLose StreamReader
                sr1.Close();

                string[] Data = list.ToArray();

                StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contacts by Elia Ritzmann\data.txt", false);

                for (int i = 0; i < Data.Length; i++)
                {
                    sw.WriteLine(Data[i]);
                }
                sw.Close();


                contactra.Loading_Data();
                contactra.Reload_List();
                contactra.Change();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Birtdate Value is Corrupted. Fix: Please reenter the Birtdate.", "Code 101", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your Data is Currupted. Fix: Delete the Contact.", "Code 102", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please do not enter a ; in a Field.", "Code 103", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Your Picure is not Supported.", "Code 104", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
