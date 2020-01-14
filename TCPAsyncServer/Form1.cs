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

namespace TCPAsyncServer
{
    public partial class Form1 : Form
    {
        TcpListener listener;
        TcpClient client;
        int port;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            
            try
            {
                port = int.Parse(tbxPort.Text);

                if (listener == null)
                {
                    listener = new TcpListener(IPAddress.Any, port);
                }
                listener.Start();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
                btnStart.Enabled = true;
                return;
            }

            StartAccepting();
        }

        public async void StartAccepting()
        {
            try
            {
                client = await listener.AcceptTcpClientAsync();

                IPEndPoint clienttmp = client.Client.LocalEndPoint as IPEndPoint;
                tbxMessage.Text += "New connection from " + clienttmp.Address.ToString() + " was initialised at " + System.DateTime.Now;
                tbxMessage.Text += Environment.NewLine;
                tbxMessage.Text += Environment.NewLine;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }

            StartReading(client);
        }

        public async void StartReading (TcpClient client)
        {
            byte[] buffer = new byte[1024];

            int n = 0;
            string message;
            try
            {
                n = await client.GetStream().ReadAsync(buffer, 0, 1024);
                message = Encoding.Unicode.GetString(buffer, 0, n);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }

            if (message.Equals("ClientClosingNow-R"))
            {
                IPEndPoint clientTmp = client.Client.LocalEndPoint as IPEndPoint;

                tbxMessage.Text += "Connection from " + clientTmp.Address.ToString() + " was terminated at " + System.DateTime.Now;
                tbxMessage.Text += Environment.NewLine;
                tbxMessage.Text += Environment.NewLine;
                client.Close();

                btnStart_Click(this, null);
            }
            else
            {
                IPEndPoint clientTmp = client.Client.LocalEndPoint as IPEndPoint;

                tbxMessage.Text += "Message received at " + System.DateTime.Now + " from " + clientTmp.Address.ToString() + ". Message contains: " + "\n";
                tbxMessage.AppendText(message);
                tbxMessage.Text += Environment.NewLine;
                tbxMessage.Text += Environment.NewLine;

                StartReading(client);
            }
        }
    }
}
