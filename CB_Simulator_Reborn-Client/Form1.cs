using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CB_Simulator_Reborn_Client
{
    public partial class CB_Simulator_Reborn_Client : Form
    {
        private const int broadcastPort = 15000;
        private readonly UdpClient broadcastReceiver = new UdpClient(broadcastPort);
        private IPAddress serverIP;
        private int serverPort;

        private bool nextMessageUserList = false;
        private List<CB_Simulator_clientInfoLight> userList;

        private TcpClient Client;

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
                Client = new TcpClient();
                await Client.ConnectAsync(serverIP, serverPort);
                StartReceiving();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }


        private async void StartReceiving ()
        {
            byte[] buffer = new byte[10360];

            int n = 0;
            string message = "";

            try
            {
                n = await Client.GetStream().ReadAsync(buffer, 0, 4096);

                if (!nextMessageUserList)
                {
                    message = Encoding.UTF8.GetString(buffer, 0, n);
                }
                else
                {
                    nextMessageUserList = false;
                    userList = Deserialize(buffer);
                    updateUserList();
                }

                if (message.Equals("R-A"))
                {
                    SendAuth();
                }
                else if (message.Equals("U-L-98759183"))
                {
                    nextMessageUserList = true;
                }
            }
            catch (Exception e)
            {
                errorHandle(e);
            }

            StartReceiving();
        }

        private async void SendAuth()
        {
            string message = "Auth: Nickname: nickname"; //Fix
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
        }



        private static List<CB_Simulator_clientInfoLight> Deserialize(byte[] userList)
        {
            List<CB_Simulator_clientInfoLight> tmpUserList = new List<CB_Simulator_clientInfoLight>();
            int index = 0;

            int userCount = BitConverter.ToInt32(userList, 0);
            while (index < userCount)
            {
                try
                {
                    int userId = BitConverter.ToInt32(userList, 4 + (4 + 512) * index);
                    string username = Encoding.UTF8.GetString(userList, 8 + (4 + 512) * index, 512);

                    //int usernameStopIndex = username.IndexOf("\\");
                    //if (usernameStopIndex > 0)
                    //{
                    //    username = username.Substring(0, usernameStopIndex + 1);
                    //}
                    username = username.Substring(0, Math.Max(0, username.IndexOf('\0')));

                    CB_Simulator_clientInfoLight tmpClient = new CB_Simulator_clientInfoLight(userId, username);
                    tmpUserList.Add(tmpClient);
                    index++;
                }
                catch
                {
                }
            }

            return tmpUserList;
        }

        private void updateUserList()
        {
            for (int i = 0; i < userList.Count; i++)
            {
                lbxUsers.Items.Add(userList[i].ClientNickname);
            }
        }

        private void errorHandle(Exception e)
        {
            //MessageBox.Show(this, e.Message, "An error has occured", MessageBoxButtons.OK);
        }
    }
}
