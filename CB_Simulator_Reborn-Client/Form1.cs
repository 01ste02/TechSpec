using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
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
            byte[] buffer = new byte[4096];

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
            /*BinaryFormatter bin = new BinaryFormatter();
            var memoryStream = new MemoryStream(userList);
            object tmp = bin.Deserialize(memoryStream);
            List<CB_Simulator_Reborn_Server.CB_Simulator_clientInfoLight> list = tmp as List<CB_Simulator_Reborn_Server.CB_Simulator_clientInfoLight>;
            //as List<CB_Simulator_Reborn_Server.CB_Simulator_clientInfoLight>*/
            MemoryStream stream = new MemoryStream(userList);
            List<CB_Simulator_clientInfoLight> list = (List<CB_Simulator_clientInfoLight>)DeserializeFromStream(stream);
            return list;
        }

        public static object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
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
