using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCPAsyncSender
{
    public partial class Form1 : Form
    {
        TcpClient client;
        int port;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            StartSending(tbxMessage.Text);
        }

        public async void Connect()
        {
            IPAddress address = IPAddress.Parse(tbxIP.Text);
            
            try
            {
                port = int.Parse(tbxPort.Text);
                await client.ConnectAsync(address, port);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (client == null || !client.Connected)
            {
                client = new TcpClient();
            }
            if (!client.Connected)
            {
                Connect();
                btnSend.Enabled = true;
                btnConnect.Enabled = false;
                btnDisconnect.Enabled = true;
            }
        }

        public async void StartSending (string message)
        {
            if (client.Connected)
            {
                byte[] data = Encoding.Unicode.GetBytes(message);

                try
                {
                    await client.GetStream().WriteAsync(data, 0, data.Length);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, Text);
                    return;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                StartSending("ClientClosingNow-R");
                client.Close();
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            if (client != null)
            {
                StartSending("ClientClosingNow-R");
                client.Close();
                btnDisconnect.Enabled = false;
                btnConnect.Enabled = true;
            }
        }
    }
}
