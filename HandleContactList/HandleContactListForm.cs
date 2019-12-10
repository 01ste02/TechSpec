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

namespace HandleContactList
{
    public partial class HandleContactListForm : Form
    {
        public HandleContactListForm()
        {
            InitializeComponent();
        }

        //List<string[]> contactList = new List<string[]>();
        List<Contact> contactList = new List<Contact>();

        private class Contact
        {
            private string firstname;
            private string surname;
            private string email;
            private string phone;

            public Contact()
            {
                
            }

            public string Firstname
            {
                get
                {
                    return firstname;
                }
                set
                {
                    firstname = value;
                }
            }

            public string Surname
            {
                get
                {
                    return surname;
                }
                set
                {
                    surname = value;
                }
            }

            public string Email
            {
                get
                {
                    return email;
                }
                set
                {
                    email = value;
                }
            }

            public string Phone
            {
                get
                {
                    return phone;
                }
                set
                {
                    phone = value;
                }
            }

            public override string ToString()
            {
                return Firstname + " " + Surname;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();

            if (result == DialogResult.OK)
            {
                int lineCount = File.ReadLines(dlgOpen.FileName).Count();

                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                StreamReader reader = new StreamReader(inStream);

                contactList.Clear();

                for (int i = 0; i < (lineCount / 4); i++)
                {
                    /*string[] tmp = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        tmp[j] = reader.ReadLine();
                    }*/

                    //contactList.Add(tmp);
                    //lbxContacts.Items.Add(contactList.ElementAt(i)[0] + " " + contactList.ElementAt(i)[1]);

                    Contact tmpContact = new Contact();
                    tmpContact.Firstname = reader.ReadLine();
                    tmpContact.Surname = reader.ReadLine();
                    tmpContact.Email = reader.ReadLine();
                    tmpContact.Phone = reader.ReadLine();

                    contactList.Add(tmpContact);
                }

                lbxContacts.Items.Clear();

                for (int i = 0; i < contactList.Count; i++)
                {
                    lbxContacts.Items.Add(contactList.ElementAt(i).ToString());
                }

                reader.Dispose();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSave.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream outStream = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8);

                for (int i = 0; i < contactList.Count; i++)
                {
                    /*for (int j = 0; j < 4; j++)
                    {
                        writer.WriteLine(contactList.ElementAt(i)[j]);
                    }*/
                    writer.WriteLine(contactList.ElementAt(i).Firstname);
                    writer.WriteLine(contactList.ElementAt(i).Surname);
                    writer.WriteLine(contactList.ElementAt(i).Email);
                    writer.WriteLine(contactList.ElementAt(i).Phone);

                }
                writer.Dispose();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFirstname.Text) && !string.IsNullOrEmpty(tbxSurname.Text) && !string.IsNullOrEmpty(tbxEmail.Text) && !string.IsNullOrEmpty(tbxPhone.Text))
            {
                /*string[] tmp = new string[4];
                tmp[0] = tbxFirstname.Text;
                tmp[1] = tbxSurname.Text;
                tmp[2] = tbxEmail.Text;
                tmp[3] = tbxPhone.Text;
                contactList.Add(tmp);*/

                Contact tmpContact = new Contact();
                tmpContact.Firstname = tbxFirstname.Text;
                tmpContact.Surname = tbxSurname.Text;
                tmpContact.Email = tbxEmail.Text;
                tmpContact.Phone = tbxPhone.Text;

                contactList.Add(tmpContact);

                lbxContacts.Items.Clear();
                
                for (int i = 0; i < contactList.Count; i++)
                {
                    lbxContacts.Items.Add(contactList.ElementAt(i).ToString());
                }
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            contactList.RemoveAt(lbxContacts.SelectedIndex);

            lbxContacts.Items.Clear();

            for (int i = 0; i < contactList.Count; i++)
            {
                lbxContacts.Items.Add(contactList.ElementAt(i).ToString());
            }
        }

        private void lbxContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxFirstname.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Firstname;

            tbxSurname.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Surname;

            tbxEmail.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Email;

            tbxPhone.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Phone;
        }

        private void openContactFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();

            if (result == DialogResult.OK)
            {
                int lineCount = File.ReadLines(dlgOpen.FileName).Count();

                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                StreamReader reader = new StreamReader(inStream);

                contactList.Clear();

                for (int i = 0; i < (lineCount / 4); i++)
                {
                    /*string[] tmp = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        tmp[j] = reader.ReadLine();
                    }*/

                    //contactList.Add(tmp);
                    //lbxContacts.Items.Add(contactList.ElementAt(i)[0] + " " + contactList.ElementAt(i)[1]);

                    Contact tmpContact = new Contact();
                    tmpContact.Firstname = reader.ReadLine();
                    tmpContact.Surname = reader.ReadLine();
                    tmpContact.Email = reader.ReadLine();
                    tmpContact.Phone = reader.ReadLine();

                    contactList.Add(tmpContact);
                }

                lbxContacts.Items.Clear();

                for (int i = 0; i < contactList.Count; i++)
                {
                    lbxContacts.Items.Add(contactList.ElementAt(i).ToString());
                }

                reader.Dispose();
            }
        }

        private void saveContactFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSave.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream outStream = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8);

                for (int i = 0; i < contactList.Count; i++)
                {
                    /*for (int j = 0; j < 4; j++)
                    {
                        writer.WriteLine(contactList.ElementAt(i)[j]);
                    }*/
                    writer.WriteLine(contactList.ElementAt(i).Firstname);
                    writer.WriteLine(contactList.ElementAt(i).Surname);
                    writer.WriteLine(contactList.ElementAt(i).Email);
                    writer.WriteLine(contactList.ElementAt(i).Phone);

                }
                writer.Dispose();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
