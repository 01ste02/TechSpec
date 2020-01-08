using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace TCPServer
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        TcpClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;

            listener = new TcpListener(IPAddress.Any, int.Parse(tbxPort.Text));
            listener.Start();

            client = listener.AcceptTcpClient();

            byte[] inStream = new byte[256];
            int numBytes = client.GetStream().Read(inStream, 0, inStream.Length);

            tbxMessage.Text += DateTime.Now.ToString("h:mm:ss tt") + ": " + Encoding.Unicode.GetString(inStream, 0, numBytes) + Environment.NewLine;

            client.Close();
            listener.Stop();

            btnStart.Enabled = true;
        }
    }
}
