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

        public CB_Simulator_Reborn_Server()
        {
            InitializeComponent();
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
            try
            {
                broadcaster.EnableBroadcast = true;
                byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastMessage);
                await broadcaster.SendAsync(broadcastBytes, broadcastBytes.Length, broadcastEndpoint);
            }
            catch (Exception e)
            {
                if (broadcaster.EnableBroadcast == true)
                {
                    errorHandle(e);
                }
            }
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
                if (broadcaster.EnableBroadcast == true) //If this is false, the server shouldn't be accepting clients. Error generated in fault.
                {
                    errorHandle(e);
                }
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

                for (int m = 0; m < clientList.Count; m++)
                {
                    SendUserList(clientList[m].Client);
                }
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

                updateUserList();

                Timer userListDelayTimer = new Timer();
                userListDelayTimer.Tick += (sender, e) => SendUserListSerialized(sender, e, client);
                userListDelayTimer.Interval = 1000; // in miliseconds
                userListDelayTimer.Start();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        private async void SendUserListSerialized (object sender, EventArgs e, TcpClient client)
        {
            try
            {
                Timer tmp = sender as Timer;
                tmp.Stop();
                

                byte[] message = SerializeUserList(clientListLight);
                await client.GetStream().WriteAsync(message, 0, message.Length);
                ReceiveFromClient(client);
            }
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private void updateUserList()
        {
            lbxUsers.Items.Clear();
            for (int i = 0; i < clientList.Count; i++)
            {
                lbxUsers.Items.Add(clientList[i].ClientNickname);
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

        private async void ReceiveFromClient(TcpClient client)
        {
            byte[] buffer = new byte[10360];
            int n = 0;
            string message = "";

            try
            {
                n = await client.GetStream().ReadAsync(buffer, 0, 10360);
                message = Encoding.UTF8.GetString(buffer, 0, n);
                
                if (message.Equals("Disconnecting"))
                {
                    IPEndPoint tmpEndpoint = client.Client.LocalEndPoint as IPEndPoint;
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].Client == client)
                        {
                            lbxConsole.Items.Add(DateTime.Now + ": Client with ip " + clientList[i].ClientIP + " and username " + clientList[i].ClientNickname + " disconnected");
                            clientList.RemoveAt(i);
                            clientListLight.RemoveAt(i);
                            updateUserList();

                            for (int m = 0; m < clientList.Count; m++)
                            {
                                SendUserList(clientList[m].Client);
                            }
                            break;
                        }
                    }
                    client.Close();

                }
                else if (message.Substring(0, 3).Equals("C-M")) 
                {
                    ChatMessage chatMessage = new ChatMessage("None", "None");

                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].Client == client)
                        {
                            chatMessage = new ChatMessage(clientList[i].ClientNickname, message.Substring(3));
                            break;
                        }
                    }

                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].Client != client)
                        {
                            ForwardMessageToClients(clientList[i].Client, chatMessage);
                        }
                    }

                    lbxConsole.Items.Add(DateTime.Now + " " + chatMessage.FromUser + ": " + chatMessage.Message);
                }

                if (client.Connected)
                {
                    ReceiveFromClient(client);
                }
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        public async void ForwardMessageToClients (TcpClient client, ChatMessage message)
        {
            try
            {
                byte[] messageHeader = Encoding.UTF8.GetBytes("C-M");
                await client.GetStream().WriteAsync(messageHeader, 0, messageHeader.Length);

                Timer forwardMessageTimer = new Timer();
                forwardMessageTimer.Tick += (sender, e) => ForwardSerializedMessageToClients(sender, e, client, message);
                forwardMessageTimer.Interval = 1000; // in miliseconds
                forwardMessageTimer.Start();
            }
            catch (Exception e)
            {
                errorHandle(e);
            }
        }

        public async void ForwardSerializedMessageToClients (object sender, EventArgs e, TcpClient client, ChatMessage message)
        {
            try
            {
                Timer tmp = sender as Timer;
                tmp.Stop();


                byte[] chatMessage = message.ToByteArray();
                await client.GetStream().WriteAsync(chatMessage, 0, chatMessage.Length);
                ReceiveFromClient(client);
            }
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        public void errorHandle(Exception e)
        {
            MessageBox.Show(this, e.Message, "An error has occured in the server", MessageBoxButtons.OK);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            InitBroadcastTimer();
            StartListening();

            btnStopServer.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnClearAll.Enabled = true;
            btnClearConsole.Enabled = true;
            btnKick.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            broadcastTimer.Stop();
            serverListener.Stop();
            broadcaster.EnableBroadcast = false;

            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private async void btnKick_Click(object sender, EventArgs e)
        {
            if (lbxUsers.SelectedIndex >= 0)
            {
                for (int i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i].ClientNickname.Equals(lbxUsers.Items[lbxUsers.SelectedIndex]))
                    {
                        TcpClient client = clientList[i].Client;

                        byte[] message = Encoding.UTF8.GetBytes("K-D");
                        await client.GetStream().WriteAsync(message, 0, message.Length);

                        client.Close();

                        MessageBox.Show(this, clientList[i].ClientNickname + " has been kicked from the server.", "User Kicked", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        lbxConsole.Items.Add(DateTime.Now + ": Client with ip " + clientList[i].ClientIP + " and username " + clientList[i].ClientNickname + " disconnected");
                        clientList.RemoveAt(i);
                        clientListLight.RemoveAt(i);
                        updateUserList();

                        for (int m = 0; m < clientList.Count; m++)
                        {
                            SendUserList(clientList[m].Client);
                        }

                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show(this, "Please select a user to kick!", "Select User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void btnStopServer_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                TcpClient client = clientList[i].Client;

                byte[] message = Encoding.UTF8.GetBytes("S-C");
                await client.GetStream().WriteAsync(message, 0, message.Length);

                client.Close();

                lbxConsole.Items.Add(DateTime.Now + ": Client with ip " + clientList[i].ClientIP + " and username " + clientList[i].ClientNickname + " disconnected");
                clientList.RemoveAt(i);
                clientListLight.RemoveAt(i);
                updateUserList();

                for (int m = 0; m < clientList.Count; m++)
                {
                    SendUserList(clientList[m].Client);
                }
            }

            btnStopServer.Enabled = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnClearAll.Enabled = false;
            btnClearConsole.Enabled = true;
            btnKick.Enabled = false;

            MessageBox.Show(this, "The server-close signal has been sent", "Server Close", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            lbxConsole.Items.Clear();
            lbxConsole.Items.Add("Observe that the console has only been cleared for the server, and not the clients. Use the \"Clear All\" button to clear the chat for all clients.");
        }

        private async void btnClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clientList.Count; i++)
            {
                TcpClient client = clientList[i].Client;

                byte[] message = Encoding.UTF8.GetBytes("C-C");
                await client.GetStream().WriteAsync(message, 0, message.Length);
            }

            lbxConsole.Items.Add(DateTime.Now + ": All client chats have been cleared");
            MessageBox.Show(this, "All chats have been force-cleared.", "Chat Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnSaveLog_Click(object sender, EventArgs e)
        {
            DialogResult result = saveConsoleDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                FileStream outStream = new FileStream(saveConsoleDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8);

                for (int i = 0; i < lbxConsole.Items.Count; i++)
                {
                    await writer.WriteLineAsync(lbxConsole.Items[i].ToString());
                }

                writer.Dispose();
                MessageBox.Show(this, "File saved", "Console Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this, "Please select a save location and file name to save the console to.", "Saving Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxMessage.Text) && !String.IsNullOrWhiteSpace(tbxMessage.Text))
            {
                if (tbxMessage.Text.Length < 1024)
                {
                    ChatMessage messageToClients = new ChatMessage("Server", tbxMessage.Text);
                    lbxConsole.Items.Add(DateTime.Now + " Server:" + tbxMessage.Text);

                    tbxMessage.Text = "";

                    for (int i = 0; i < clientList.Count; i++)
                    {
                        ForwardMessageToClients(clientList[i].Client, messageToClients);
                    }
                }
            }
        }

        private void formClosing(object sender, FormClosedEventArgs e)
        {
            btnStopServer_Click(this, e);
        }
    }
}
