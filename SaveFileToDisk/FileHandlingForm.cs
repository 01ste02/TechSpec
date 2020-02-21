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

namespace SaveFileToDisk
{
    public partial class frmEdit : Form
    {
        public frmEdit()
        {
            InitializeComponent();
        }

        private void BtnReplace_Click(object sender, EventArgs e)
        {
            string replace = tbxReplace.Text;
            string replaceWith = tbxReplaceWith.Text;

            if (replace != null && replaceWith != null && !replace.Equals(""))
            {
                string inText = rtbText.Text;

                string outText = inText.Replace(replace, replaceWith);

                rtbText.Text = outText;
            }
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgOpen.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream inStream = new FileStream(dlgOpen.FileName, FileMode.Open, FileAccess.Read);

                StreamReader reader = new StreamReader(inStream);

                string inText = reader.ReadToEnd();

                rtbText.Text = inText;

                reader.Dispose();
            }
        }

        
        private void BtnSave_Click(object sender, EventArgs e)
        {
            DialogResult result = dlgSave.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream outStream = new FileStream(dlgSave.FileName, FileMode.OpenOrCreate, FileAccess.Write);

                StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8);

                //writer.NewLine = true;

                writer.Write(rtbText.Text);

                writer.Dispose();
            }
        }
    }
}
