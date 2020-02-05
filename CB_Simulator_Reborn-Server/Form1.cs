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
using System.Xml.Serialization;

namespace CB_Simulator_Reborn_Server
{
    public partial class CB_Simulator_Reborn_Server : Form
    {
        //Broadcast variables
        private const int serverPort = 11563;
        private IPAddress serverIP = IPAddress.Any;
        private UdpClient broadcaster = new UdpClient();
        private const int broadcastPort = 15000;
        private IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
        private string broadcastMessage = "Server is on: " + serverPort.ToString();

        private Timer broadcastTimer;

        //TCP client variables
        private TcpListener serverListener;
        private List<CB_Simulator_clientInfo> clientList = new List<CB_Simulator_clientInfo>();
        public List<CB_Simulator_clientInfoLight> clientListLight = new List<CB_Simulator_clientInfoLight>();
        private List<TcpClient> incompleteClients = new List<TcpClient>();

        string authRequestMessage = "R-A";

        private Timer userListDelayTimer;
        private TcpClient tmpUserListClient;

        public CB_Simulator_Reborn_Server()
        {
            InitializeComponent();
            InitBroadcastTimer();
            StartListening();
        }

        public void InitBroadcastTimer()
        {
            broadcastTimer = new Timer();
            broadcastTimer.Tick += new EventHandler(broadcastTimer_Tick);
            broadcastTimer.Interval = 1000; // in miliseconds
            broadcastTimer.Start();
        }

        private void broadcastTimer_Tick(object sender, EventArgs e)
        {
            Broadcaster();
        }

        public async void Broadcaster()
        {
            broadcaster.EnableBroadcast = true;
            byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastMessage);
            await broadcaster.SendAsync(broadcastBytes, broadcastBytes.Length, broadcastEndpoint);
        }

        public void StartListening()
        {
            try
            {
                serverListener = new TcpListener(IPAddress.Any, serverPort);
                serverListener.Start();
                AcceptClients();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        public async void AcceptClients()
        {
            try
            {
                TcpClient tmpClient = await serverListener.AcceptTcpClientAsync();
                incompleteClients.Add(tmpClient);
                SendAuthRequest(tmpClient);
                AcceptClients();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        public async void SendAuthRequest(TcpClient client)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(authRequestMessage);
                await client.GetStream().WriteAsync(message, 0, message.Length);

                ReceiveAuth(client);
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        private async void ReceiveAuth(TcpClient client)
        {
            byte[] buffer = new byte[1024];
            int n = 0;
            string message = "";

            try
            {
                n = await client.GetStream().ReadAsync(buffer, 0, 1024);
                message = Encoding.UTF8.GetString(buffer, 0, n);
                string nickname = message.Substring(15);

                IPEndPoint tmpEndpoint = client.Client.LocalEndPoint as IPEndPoint;
                CB_Simulator_clientInfo tmpClient = new CB_Simulator_clientInfo(client, tmpEndpoint.Address, clientList.Count + 1, DateTime.Now, nickname);
                CB_Simulator_clientInfoLight tmpClientLight = new CB_Simulator_clientInfoLight(clientList.Count + 1, nickname);
                clientList.Add(tmpClient);
                clientListLight.Add(tmpClientLight);
                Console.WriteLine("Added new client");

                lbxConsole.Items.Add(DateTime.Now + ": New client connected from " + tmpClient.ClientIP + " with the username " + tmpClient.ClientNickname);
                SendUserList(client);
            }
            catch (Exception e)
            {
                errorHandle(e);
            }

        }

        private async void SendUserList (TcpClient client)
        {
            try
            {
                byte[] messageHeader = Encoding.UTF8.GetBytes("U-L-98759183");

                await client.GetStream().WriteAsync(messageHeader, 0, messageHeader.Length);

                tmpUserListClient = client;

                userListDelayTimer = new Timer();
                userListDelayTimer.Tick += new EventHandler(SendUserListSerialized);
                userListDelayTimer.Interval = 1000; // in miliseconds
                userListDelayTimer.Start();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        private async void SendUserListSerialized (object sender, EventArgs e)
        {
            try
            {
                userListDelayTimer.Stop();
                TcpClient client = tmpUserListClient;
                tmpUserListClient = null;

                byte[] message = SerializeUserList(clientListLight);
                await client.GetStream().WriteAsync(message, 0, message.Length);
            }
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }




        private static byte[] SerializeUserList(List<CB_Simulator_clientInfoLight> userList)
        {
            byte[] tmp = new byte[userList.Count * (512 + 4) + 4];
            BitConverter.GetBytes(userList.Count).CopyTo(tmp, 0);
            for (int i = 0; i < userList.Count; i++)
            {
                byte[] currentUser = userList[i].ToByteArray();
                currentUser.CopyTo(tmp, i * (512 + 4) + 4);
            }

            return tmp;
        }

        public void errorHandle(Exception e)
        {
            MessageBox.Show(this, e.Message, "An error has occured", MessageBoxButtons.OK);
        }

        /*
         * Server broadcastar, klient hittar ip och får port från broadcast-meddelande. Klient kan även speca IP
         * Server lyssnar, lägger till klienter i lista med klient-klass (innehåller info. T.ex ip, id, connection time, nickname, last message (?), last seen time (?))
         * Tar emot data från alla klienter, sänder vidare till alla andra. Sänder meddelande då någon ansluter / kopplar från
         * 
         * Klient lyssnar efter broadcast. IP kan även specas. 
         * Ansluter till port från broadcast, sänder port den lyssnar på. Server ansluter.
         * Klient tar emot lista med klient-klass. Får meddelande då någon ansluter/kopplar från
         * */
    }
}
