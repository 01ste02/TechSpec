using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace UdpClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnRecieve_Click(object sender, EventArgs e)
        {
            IPEndPoint host = new IPEndPoint(IPAddress.Any, 0);

            System.Net.Sockets.UdpClient client = new System.Net.Sockets.UdpClient(int.Parse(tbxPort.Text));
            byte[] message = client.Receive(ref host);

            tbxMessage.Text += DateTime.Now.ToString("h:mm:ss tt") + ": " + Encoding.Unicode.GetString(message);
            tbxMessage.Text += Environment.NewLine;

            client.Dispose();
        }
    }
}
