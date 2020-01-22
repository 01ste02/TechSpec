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

namespace CB_Simulator_Reborn_Server
{
    public partial class Form1 : Form
    {
        //Broadcast variables
        const int serverPort = 11563;
        IPAddress serverIP = IPAddress.Any;
        UdpClient broadcaster = new UdpClient();
        const int broadcastPort = 15000;
        IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
        string broadcastMessage = "Server is on: " + serverPort.ToString();

        private Timer broadcastTimer;

        //TCP client variables
        TcpListener serverListener;
        TcpClient serverSender;
        List<CB_Simulator_clientInfo> clientList = new List<CB_Simulator_clientInfo>();
        List<TcpClient> incompleteClients = new List<TcpClient>();

        string authRequestMessage = "R-A";

        public Form1()
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
                serverSender = new TcpClient();
                IPEndPoint tmpEndpoint = client.Client.LocalEndPoint as IPEndPoint;
                await serverSender.ConnectAsync(tmpEndpoint.Address.ToString(), serverPort + 1);

                byte[] message = Encoding.UTF8.GetBytes(authRequestMessage);
                await serverSender.GetStream().WriteAsync(message, 0, message.Length);
                Console.WriteLine("Test4");

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
            Console.WriteLine("Test5");
            int n = 0;
            string message = "";

            try
            {
                n = await client.GetStream().ReadAsync(buffer, 0, 1024);
                message = Encoding.UTF8.GetString(buffer, 0, n);
                string nickname = message.Substring(15);

                IPEndPoint tmpEndpoint = client.Client.LocalEndPoint as IPEndPoint;
                CB_Simulator_clientInfo tmpClient = new CB_Simulator_clientInfo(client, tmpEndpoint.Address, clientList.Count + 1, DateTime.Now, nickname);
                clientList.Add(tmpClient);
                Console.WriteLine("Added new client");
            }
            catch (Exception e)
            {
                errorHandle(e);
            }

            

            ReceiveAuth(client);
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
