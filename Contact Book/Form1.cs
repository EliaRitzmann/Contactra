using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using System.IO;
using System.Reflection;

namespace Contact_Book
{

    public partial class Contactra : Form
    {
        public string rootPath;
        public string datapath;
        public string picturepath;
        //Data
        public string[] Data = new string[1];
        public string[] Split = new string[27];
        bool creatmode = false;
        private Bitmap Bitmap;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public Contactra()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Read Data from File
            Data = Loading_Data();
            Reload_List();
            listbox.SelectedIndex = 0;
            Change();
        }

        public string[] Loading_Data()
        {
            rootPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Contacts by Elia Ritzmann";
            //string rootPath = Path.Combine(Environment.CurrentDirectory, @"Data");
            //string rootPath = Path.Combine(@"C:\Program Files(x86)\Contacts by Elia Ritzmann\Contacts\Data");
            datapath = Path.Combine(rootPath, @"data.txt");
            picturepath = Path.Combine(rootPath, @"pictures");
            

            Directory.CreateDirectory(rootPath);
            Directory.CreateDirectory(picturepath);
            if(!File.Exists(datapath))
            {
                StreamWriter sw = File.CreateText(datapath);
                sw.Close();
            }
            
            
            StreamReader sr = new StreamReader(datapath);

            string line;
            var list = new List<string>();
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(line);
            }
            string[] Data = list.ToArray();
            //CLose StreamReader
            sr.Close();

            if(Data.Length == 0)
            {
                Data = new string[1];
                Data[0] = "Firstname;Name;Nickname;false;13.04.2021 00:00:00;Street;Postal;City;Country;+             ;1;+             ;1;+             ;1;+             ;1; ;2; ;2; ;2; ;2;Demo;false";
            }
            //Sort Array 
            Array.Sort(Data);
            return Data;
        }

        public void Reload_List()
        {
            Data = Loading_Data();
            //Clear Listbox
            listbox.Items.Clear();
            //Import in ListBox
            for (int i = 0; i < Data.Length; i++)
            {
                Split = Data[i].Split(';');
                if (Split[3] == "true")
                {
                    listbox.Items.Add(Split[2]);
                }
                else
                {
                    listbox.Items.Add(Split[0] + " " + Split[1]);
                }
            }

            listbox.SelectedIndex = 0;
        }

        private void Save_Data()
        {
            StreamWriter sw = new StreamWriter(datapath, false);
            //Save Specific Data



            //Save AllData
            for(int i = 0; i < Data.Length; i++)
            {
                sw.WriteLine(Data[i]);
            }
            sw.Close();
        }

        private void pnl_header_Paint(object sender, PaintEventArgs e)
        {
        }

        private void btn_CurrentContact_Click(object sender, EventArgs e)
        {
            creatmode = false;
            btn_CurrentContact.Enabled = false;
            btn_newContact.Enabled = true;
            btn_CurrentContact.BackColor = Color.White;
            btn_newContact.BackColor = Color.FromArgb(248, 248, 248);

            Change();
        }

        private void btn_newContact_Click(object sender, EventArgs e)
        {
            creatmode = true;
            btn_CurrentContact.Enabled = true;
            btn_newContact.Enabled = false;
            btn_newContact.BackColor = Color.White;
            btn_CurrentContact.BackColor = Color.FromArgb(248, 248, 248);

            AddContact();
        }

        private void pnl_header_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void btn_Close_MouseHover(object sender, EventArgs e)
        {

        }

        private void btn_addnew_Click(object sender, EventArgs e)
        {
            btn_CurrentContact.Enabled = true;
            btn_newContact.Enabled = false;
            btn_newContact.BackColor = Color.White;
            btn_CurrentContact.BackColor = Color.FromArgb(248, 248, 248);

            AddContact();
        }

        private void listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            creatmode = false;
            btn_CurrentContact.Enabled = false;
            btn_newContact.Enabled = true;
            btn_CurrentContact.BackColor = Color.White;
            btn_newContact.BackColor = Color.FromArgb(248, 248, 248);

            Change();
        }
        public void Change()
        {
            //Vorname;Nachname;Spitzname;true;geburtsdatum;telefonpriv;emailpriv;telefongeschäft;telefongeschäft;notizen
            //SplitArray
            Split = Data[listbox.SelectedIndex].Split(';');
            //ContactData
            try
            {
                lbl_wholeName.Text = Split[0] + " " + Split[1];
                txb_Firstname.Text = Split[0];
                txb_Name.Text = Split[1];
                txb_Nickname.Text = Split[2];
                if (Split[3] == "true")
                {
                    checkBox1.Checked = true;
                    btn_CurrentContact.Text = Split[2];
                }
                else
                {
                    checkBox1.Checked = false;
                    btn_CurrentContact.Text = Split[0] + " " + Split[1];
                }

                try
                {
                    dateTimePicker1.Value = Convert.ToDateTime(Split[4]);
                }
                catch
                {
                    MessageBox.Show("Data curruption Code 101", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dateTimePicker1.Value = DateTime.Now;
                }
                txb_Street.Text = Split[5];
                txb_Postal.Text = Split[6];
                txb_City.Text = Split[7];
                txb_Country.Text = Split[8];

                //Phone and Email -> Explain Part in Portfolio :)
                mtb_Phone1.Text = Split[9];
                mtb_Phone2.Text = Split[11];
                mtb_Phone3.Text = Split[13];
                mtb_Phone4.Text = Split[15];

                cmb_Phone1.SelectedIndex = Convert.ToInt32(Split[10]);
                cmb_Phone2.SelectedIndex = Convert.ToInt32(Split[12]);
                cmb_Phone3.SelectedIndex = Convert.ToInt32(Split[14]);
                cmb_Phone4.SelectedIndex = Convert.ToInt32(Split[16]);

                cmb_Phone_Change();

                txb_Email1.Text = Split[17];
                txb_Email2.Text = Split[19];
                txb_Email3.Text = Split[21];
                txb_Email4.Text = Split[23];

                cmb_Email1.SelectedIndex = Convert.ToInt32(Split[18]);
                cmb_Email2.SelectedIndex = Convert.ToInt32(Split[20]);
                cmb_Email3.SelectedIndex = Convert.ToInt32(Split[22]);
                cmb_Email4.SelectedIndex = Convert.ToInt32(Split[24]);

                cmb_Email_Change();

                txb_Infos.Text = Split[25];

                if (Split[26] != "false")
                {
                    if (Bitmap != null)
                    {
                        Bitmap.Dispose();
                    }
                    Bitmap = new Bitmap(Split[26]);
                    pcb_ProfilePicture.Image = Bitmap;
                }
                else
                {
                    pcb_ProfilePicture.Image = Properties.Resources._default;
                }

            }
            catch(OverflowException)
            {
                MessageBox.Show("Data curruption Code 102", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void cmb_Phone_Change()
        {
            switch (cmb_Phone1.SelectedIndex)
            {
                case 0:
                    lbl_Phone1.Text = "Phone (Mobile):";
                    break;
                case 1:
                    lbl_Phone1.Text = "Phone (Private):";
                    break;
                case 2:
                    lbl_Phone1.Text = "Phone (Work):";
                    break;
                case 3:
                    lbl_Phone1.Text = "Phone (Other):";
                    break;
            }
            switch (cmb_Phone2.SelectedIndex)
            {
                case 0:
                    lbl_Phone2.Text = "Phone (Mobile):";
                    break;
                case 1:
                    lbl_Phone2.Text = "Phone (Private):";
                    break;
                case 2:
                    lbl_Phone2.Text = "Phone (Work):";
                    break;
                case 3:
                    lbl_Phone2.Text = "Phone (Other):";
                    break;
            }
            switch (cmb_Phone3.SelectedIndex)
            {
                case 0:
                    lbl_Phone3.Text = "Phone (Mobile):";
                    break;
                case 1:
                    lbl_Phone3.Text = "Phone (Private):";
                    break;
                case 2:
                    lbl_Phone3.Text = "Phone (Work):";
                    break;
                case 3:
                    lbl_Phone3.Text = "Phone (Other):";
                    break;
            }
            switch (cmb_Phone4.SelectedIndex)
            {
                case 0:
                    lbl_Phone4.Text = "Phone (Mobile):";
                    break;
                case 1:
                    lbl_Phone4.Text = "Phone (Private):";
                    break;
                case 2:
                    lbl_Phone4.Text = "Phone (Work):";
                    break;
                case 3:
                    lbl_Phone4.Text = "Phone (Other):";
                    break;
            }
        }

        private void cmb_Email_Change()
        {
            switch (cmb_Email1.SelectedIndex)
            {
                case 0:
                    lbl_Email1.Text = "Email (Private):";
                    break;
                case 1:
                    lbl_Email1.Text = "Email (Work):";
                    break;
                case 2:
                    lbl_Email1.Text = "Email (Other):";
                    break;
            }
            switch (cmb_Email2.SelectedIndex)
            {
                case 0:
                    lbl_Email2.Text = "Email (Private):";
                    break;
                case 1:
                    lbl_Email2.Text = "Email (Work):";
                    break;
                case 2:
                    lbl_Email2.Text = "Email (Other):";
                    break;
            }
            switch (cmb_Email3.SelectedIndex)
            {
                case 0:
                    lbl_Email3.Text = "Email (Private):";
                    break;
                case 1:
                    lbl_Email3.Text = "Email (Work):";
                    break;
                case 2:
                    lbl_Email3.Text = "Email (Other):";
                    break;
            }
            switch (cmb_Email4.SelectedIndex)
            {
                case 0:
                    lbl_Email4.Text = "Email (Private):";
                    break;
                case 1:
                    lbl_Email4.Text = "Email (Work):";
                    break;
                case 2:
                    lbl_Email4.Text = "Email (Other):";
                    break;
            }
        }

        private void AddContact()
        {
            txb_Firstname.Text = "";
            txb_Name.Text = "";
            txb_Nickname.Text = "";
            checkBox1.Checked = false;
            dateTimePicker1.Value = DateTime.Now;

            txb_Street.Text = "";
            txb_Postal.Text = "";
            txb_City.Text = "";
            txb_Country.Text = "";

            mtb_Phone1.Text = "";
            mtb_Phone2.Text = "";
            mtb_Phone3.Text = "";
            mtb_Phone4.Text = "";

            txb_Email1.Text = "";
            txb_Email2.Text = "";
            txb_Email3.Text = "";
            txb_Email4.Text = "";



            txb_Infos.Text = "";
            lbl_wholeName.Text = "new Contact";

            creatmode = true;

            for(int i = 0; i < Split.Length; i++)
            {
                Split[i] = " ";
            }
            Split[26] = "false";
            pcb_ProfilePicture.Image = Properties.Resources._default;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Save();
        }
        private void Save()
        {
            if (creatmode == true)
            {
                StreamWriter sw = new StreamWriter(datapath, false);

                for (int i = 0; i < Data.Length; i++)
                {
                    sw.WriteLine(Data[i]);
                }

                GetData();

                for (int j = 0; j < Split.Length; j++)
                {
                    if (Split[j].Contains(";"))
                    {
                        MessageBox.Show("Data Error Code 003", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Split[j] = "ERROR";
                    }
                }

                string Person = string.Join(";", Split);
                Data[listbox.SelectedIndex] = Person;

                sw.WriteLine(Person);
                sw.Close();

                Loading_Data();

                Reload_List();
                listbox.SelectedIndex = 0;
                Change();
            }
            else
            {
                GetData();

                for (int j = 0; j < Split.Length; j++)
                {
                    if (Split[j].Contains(";"))
                    {
                        MessageBox.Show("Data Error Code 003", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Split[j] = "ERROR";
                    }
                }

                string Person = string.Join(";", Split);
                Data[listbox.SelectedIndex] = Person;
                Save_Data();
                Reload_List();
                Change();
            }
        }

        private void GetData()
        {
            Split[0] = txb_Firstname.Text;
            Split[1] = txb_Name.Text;
            Split[2] = txb_Nickname.Text;
            if (checkBox1.Checked == true)
            {
                Split[3] = "true";
            }
            else
            {
                Split[3] = "false";
            }
            Split[4] = Convert.ToString(dateTimePicker1.Value);

            Split[5] = txb_Street.Text;
            Split[6] = txb_Postal.Text;
            Split[7] = txb_City.Text;
            Split[8] = txb_Country.Text;

            Split[9] = mtb_Phone1.Text;
            Split[11] = mtb_Phone2.Text;
            Split[13] = mtb_Phone3.Text;
            Split[15] = mtb_Phone4.Text;

            Split[10] = Convert.ToString(cmb_Phone1.SelectedIndex);
            Split[12] = Convert.ToString(cmb_Phone2.SelectedIndex);
            Split[14] = Convert.ToString(cmb_Phone3.SelectedIndex);
            Split[16] = Convert.ToString(cmb_Phone4.SelectedIndex);

            Split[17] = txb_Email1.Text;
            Split[19] = txb_Email2.Text;
            Split[21] = txb_Email3.Text;
            Split[23] = txb_Email4.Text;

            Split[18] = Convert.ToString(cmb_Email1.SelectedIndex);
            Split[20] = Convert.ToString(cmb_Email2.SelectedIndex);
            Split[22] = Convert.ToString(cmb_Email3.SelectedIndex);
            Split[24] = Convert.ToString(cmb_Email4.SelectedIndex);

            Split[25] = txb_Infos.Text;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Split[3] = "true";
            }
            else if (checkBox1.Checked == false)
            {
                Split[3] = "false";
            }



        }

        private void txb_Firstname_TextChanged(object sender, EventArgs e)
        {
            if(creatmode == true)
            {
                lbl_wholeName.Text = txb_Firstname.Text + " " + txb_Name.Text;
            }
        }

        private void txb_Name_TextChanged(object sender, EventArgs e)
        {
            if(creatmode == true)
            {
                lbl_wholeName.Text = txb_Firstname.Text + " " + txb_Name.Text;
            }
        }

        private void txb_Nickname_TextChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void txb_privateTelephone_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_PrivateEMail_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_Telephone_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_EMail_TextChanged(object sender, EventArgs e)
        {
        }

        private void txb_Infos_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Dialog
            if (MessageBox.Show("Do you really want to delet this Contact?",
                                        "Delete?",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                pcb_ProfilePicture.Image = Properties.Resources._default;
                string dpath = Split[26];
                //Delete Data
                for (int a = listbox.SelectedIndex; a < Data.Length - 1; a++)
                {
                    Data[a] = Data[a + 1];
                }
                Array.Resize(ref Data, Data.Length - 1);

                Save_Data();
                Reload_List();
                Change();
                //delete file
                //File.Delete(dpath);
            }
            


        }

        private void cmb_Phone1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Phone_Change();
        }

        private void cmb_Phone2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Phone_Change();
        }

        private void cmb_Phone3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Phone_Change();
        }

        private void cmb_Phone4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Phone_Change();
        }

        private void cmb_Email1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Email_Change();
        }

        private void cmb_Email2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Email_Change();
        }

        private void cmb_Email3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Email_Change();
        }

        private void cmb_Email4_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmb_Email_Change();
        }

        private void pcb_ProfilePicture_Click(object sender, EventArgs e)
        {
            string dpath = Path.Combine(picturepath, $"{txb_Firstname.Text}{txb_Name.Text}.JPG");

            if (Split[26] != "false")
            {
                if (MessageBox.Show("Do you want to delete your Profile Picture?",
                                         "Change Picture?",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    pcb_ProfilePicture.Image = Properties.Resources._default;
                    //File.Delete(Split[26]);
                    Split[26] = "false";
                    Save();
                }
            }
            else
            {
                if (MessageBox.Show("Do you want to add a profile picture?",
                                        "Change Picture?",
                                        MessageBoxButtons.YesNo,
                                        MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OpenFileDialog pfd = new OpenFileDialog();
                    pfd.Filter = "Image Files(*.JPG)|*.JPG|All files (*.*)|*.*";

                    if (pfd.ShowDialog() == DialogResult.OK)
                    {
                        string path = pfd.FileName;
                        try
                        {
                            //Name the File
                            if (Bitmap != null)
                            {
                                Bitmap.Dispose();
                            }
                            //deletes the Old File
                            bool close = false;
                            int i = 1;
                            do
                            {
                                if (File.Exists(dpath))
                                {
                                    dpath = Path.Combine(picturepath, $"{txb_Firstname.Text}{txb_Name.Text}_{i}.JPG");
                                }
                                else
                                {
                                    close = true;
                                    //Copies the new File
                                    File.Copy(path, dpath);
                                    //Writes the path to Array
                                    Split[26] = dpath;

                                    Bitmap = new Bitmap(dpath);
                                    pcb_ProfilePicture.Image = Bitmap;
                                }
                                i++;
                            } while (close == false);
                            
                            Save();
                        }
                        catch
                        {
                            MessageBox.Show("Data Error Code 004", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Split[26] = "false";
                            pcb_ProfilePicture.Image = Properties.Resources._default;

                        }
                    }
                }
            }

            
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_Settings_Click(object sender, EventArgs e)
        {
            Settings setting = new Settings();
            setting.ShowDialog();
        }
    }
    
}
