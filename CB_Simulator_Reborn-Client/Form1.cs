using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CB_Simulator_Reborn_Client
{
    public partial class CB_Simulator_Reborn_Client : Form
    {
        private const int broadcastPort = 15000;
        private readonly UdpClient broadcastReceiver = new UdpClient(broadcastPort);
        private IPAddress serverIP;
        private int serverPort;

        private TcpClient sendClient;
        private TcpListener recvListener;
        private TcpClient recvClient;

        public CB_Simulator_Reborn_Client()
        {
            InitializeComponent();
            startBroadcastListening();
        }

        private void startBroadcastListening()
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
            serverPort = int.Parse(broadcastString.Substring(14));
            Console.WriteLine("Broadcast Received: " + broadcastString + " " + serverIP.ToString());

            broadcastReceiver.Dispose();
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
                StartReceiving();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        private async void StartReceiving ()
        {
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
            else if (message.Contains("U-L-98759183"))
            {

            }

            StartReceiving();
        }

        private async void SendAuth()
        {
            string message = "Auth: Nickname: nickname"; //Fix
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await sendClient.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
        }





        private static List<CB_Simulator_clientInfo> Deserialize(byte[] userList)
        {
            BinaryFormatter bin = new BinaryFormatter();
            var memoryStream = new MemoryStream(userList);
            List<CB_Simulator_clientInfo> tmp = bin.Deserialize(memoryStream) as List<CB_Simulator_clientInfo>;
            return tmp;
        }

        private void errorHandle(Exception e)
        {
            MessageBox.Show(this, e.Message, "An error has occured", MessageBoxButtons.OK);
        }
    }
}
