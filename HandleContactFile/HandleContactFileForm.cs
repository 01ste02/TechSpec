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

namespace HandleContactFile
{
    public partial class HandleContactFileForm : Form
    {
        public HandleContactFileForm()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            tbxFirstname.Text = "";

            tbxSurname.Text = "";

            tbxEmail.Text = "";

            tbxPhone.Text = "";

            DialogResult result = dlgOpen.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                StreamReader reader = new StreamReader(inStream);

                tbxFirstname.Text = reader.ReadLine();

                tbxSurname.Text = reader.ReadLine();

                tbxEmail.Text = reader.ReadLine();

                tbxPhone.Text = reader.ReadLine();

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

                //writer.NewLine = true;

                writer.WriteLine(tbxFirstname.Text);

                writer.WriteLine(tbxSurname.Text);

                writer.WriteLine(tbxEmail.Text);

                writer.WriteLine(tbxPhone.Text);

                writer.Dispose();
            }
        }
    }
}
