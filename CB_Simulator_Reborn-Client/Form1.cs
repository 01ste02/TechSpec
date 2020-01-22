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
        IPAddress serverIP;
        int serverPort;

        TcpClient sendClient;
        TcpListener recvListener;
        TcpClient recvClient;

        public Form1()
        {
            InitializeComponent();
            startBroadcastListening();
        }

        public void startBroadcastListening()
        {
            broadcastReceiver.EnableBroadcast = true;
            broadcastReceiver.BeginReceive(receiveBroadcast, new Object());
        }

        private void receiveBroadcast (IAsyncResult result)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, broadcastPort);
            byte[] broadcastBytes = broadcastReceiver.EndReceive(result, ref ip);
            String broadcastString = Encoding.UTF8.GetString(broadcastBytes);
            serverIP = ip.Address;
            //serverIP = IPAddress.Parse("127.0.0.1");
            serverPort = int.Parse(broadcastString.Substring(14));
            Console.WriteLine("Broadcast Received: " + broadcastString + " " + serverIP.ToString());

            ConnectToServer();
        }

        private async void ConnectToServer()
        {
            try
            {
                sendClient = new TcpClient();
                await sendClient.ConnectAsync(serverIP, serverPort);

                IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, (serverPort + 1));
                recvListener = new TcpListener(serverEndPoint);
                recvListener.Start();
                AcceptServerConnection();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        private async void AcceptServerConnection()
        {
            try
            {
                recvClient = await recvListener.AcceptTcpClientAsync();
                Console.WriteLine("Test");
                StartReceiving();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        private async void StartReceiving ()
        {
            Console.WriteLine("Test2");
            byte[] buffer = new byte[1024];

            int n = 0;
            string message = "";

            try
            {
                n = await recvClient.GetStream().ReadAsync(buffer, 0, 1024);
                message = Encoding.UTF8.GetString(buffer, 0, n);
            }
            catch (Exception e)
            {
                errorHandle(e);
            }

            if (message.Equals("R-A"))
            {
                SendAuth();
            }

            StartReceiving();
        }

        public async void SendAuth()
        {
            Console.WriteLine("Test3");
            string message = "Auth: Nickname: nickname";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await sendClient.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        public void errorHandle(Exception e)
        {
            MessageBox.Show(this, e.Message, "An error has occured", MessageBoxButtons.OK);
        }
    }
}
