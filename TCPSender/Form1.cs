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

namespace TCPSender
{
    public partial class Form1 : Form
    {
        TcpClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            IPAddress address = IPAddress.Parse(tbxIP.Text);

            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(address, int.Parse(tbxPort.Text));

            if (client.Connected)
            {
                byte[] message = Encoding.Unicode.GetBytes(tbxMessage.Text);
                client.GetStream().Write(message, 0, message.Length);
                client.Close();
            }
        }
    }
}
