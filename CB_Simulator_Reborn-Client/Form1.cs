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

namespace CB_Simulator_Reborn_Client
{
    public partial class Form1 : Form
    {
        const int broadcastPort = 15000;
        readonly UdpClient broadcastReceiver = new UdpClient(broadcastPort);
        IPEndPoint serverIP;

        public Form1()
        {
            InitializeComponent();
        }

        public async void startBroadcastListening()
        {
            broadcastReceiver.EnableBroadcast = true;
            broadcastReceiver.BeginReceive(receiveBroadcast, new Object());
        }

        private void receiveBroadcast (IAsyncResult result)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, broadcastPort);
            byte[] broadcastBytes = broadcastReceiver.EndReceive(result, ref ip);
            String broadcastString = Encoding.UTF8.GetString(broadcastBytes);
            serverIP = result.Address;
        }
    }
}
