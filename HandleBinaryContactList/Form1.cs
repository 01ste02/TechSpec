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

namespace HandleBinaryContactList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        List<Contact> contactList = new List<Contact>();

        private class Contact
        {
            private string firstname;
            private string surname;
            private string email;
            private int phone;

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

            public int Phone
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

        private void openContactFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                BinaryReader reader = new BinaryReader(inStream);

                contactList.Clear();

                bool moreContent = true;
                while (moreContent == true)
                {
                    try
                    {
                        Contact tmpContact = new Contact();
                        tmpContact.Firstname = reader.ReadString();
                        tmpContact.Surname = reader.ReadString();
                        tmpContact.Email = reader.ReadString();
                        tmpContact.Phone = reader.ReadInt32();

                        contactList.Add(tmpContact);
                    }
                    catch
                    {
                        moreContent = false;
                    }
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

                BinaryWriter writer = new BinaryWriter(outStream, Encoding.UTF8);

                for (int i = 0; i < contactList.Count; i++)
                {
                    writer.Write(contactList.ElementAt(i).Firstname);
                    writer.Write(contactList.ElementAt(i).Surname);
                    writer.Write(contactList.ElementAt(i).Email);
                    writer.Write(contactList.ElementAt(i).Phone);

                }
                writer.Dispose();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                BinaryReader reader = new BinaryReader(inStream);

                contactList.Clear();

                bool moreContent = true;
                while (moreContent == true)
                {
                    try
                    {
                        Contact tmpContact = new Contact();
                        tmpContact.Firstname = reader.ReadString();
                        tmpContact.Surname = reader.ReadString();
                        tmpContact.Email = reader.ReadString();
                        tmpContact.Phone = reader.ReadInt32();

                        contactList.Add(tmpContact);
                    }
                    catch
                    {
                        moreContent = false;
                    }
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

                BinaryWriter writer = new BinaryWriter(outStream, Encoding.UTF8);

                for (int i = 0; i < contactList.Count; i++)
                {
                    writer.Write(contactList.ElementAt(i).Firstname);
                    writer.Write(contactList.ElementAt(i).Surname);
                    writer.Write(contactList.ElementAt(i).Email);
                    writer.Write(contactList.ElementAt(i).Phone);

                }
                writer.Dispose();
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxFirstname.Text) && !string.IsNullOrEmpty(tbxSurname.Text) && !string.IsNullOrEmpty(tbxEmail.Text) && !string.IsNullOrEmpty(tbxPhone.Text))
            {

                Contact tmpContact = new Contact();
                tmpContact.Firstname = tbxFirstname.Text;
                tmpContact.Surname = tbxSurname.Text;
                tmpContact.Email = tbxEmail.Text;
                tmpContact.Phone = int.Parse(tbxPhone.Text);

                contactList.Add(tmpContact);

                lbxContacts.Items.Clear();

                for (int i = 0; i < contactList.Count; i++)
                {
                    lbxContacts.Items.Add(contactList.ElementAt(i).ToString());
                }
            }
        }

        private void lbxContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbxFirstname.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Firstname;

            tbxSurname.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Surname;

            tbxEmail.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Email;

            tbxPhone.Text = contactList.ElementAt(lbxContacts.SelectedIndex).Phone.ToString();
        }
    }
}
