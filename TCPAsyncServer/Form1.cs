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
                listener = new TcpListener(IPAddress.Any, port);
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
            try
            {
                n = await client.GetStream().ReadAsync(buffer, 0, 1024);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, Text);
                return;
            }

            tbxMessage.AppendText(Encoding.Unicode.GetString(buffer, 0, n));

            StartReading(client);

        }
    }
}
