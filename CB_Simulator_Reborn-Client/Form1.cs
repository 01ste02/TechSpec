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
        private UdpClient broadcastReceiver;
        private IPAddress serverIP;
        private int serverPort;
        private bool broadcastReceived = false;
        private Timer isBroadcastReceivedTimer;

        private bool nextMessageUserList = false;
        private bool nextMessageChatMessage = false;
        private bool alreadyConnected = false;
        private List<CB_Simulator_clientInfoLight> userList;

        private TcpClient Client;

        public CB_Simulator_Reborn_Client()
        {
            InitializeComponent();
        }

        private void startBroadcastListening()
        {
            broadcastReceiver = new UdpClient(broadcastPort);
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
            broadcastReceived = true;
        }

        private void isBroadcastReceived (object sender, EventArgs e)
        {
            if (broadcastReceived)
            {
                ConnectToServer();
                isBroadcastReceivedTimer.Stop();
            }
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
            if (Client.Connected)
            {
                byte[] buffer = new byte[10360];

                int n = 0;
                string message = "";

                try
                {
                    n = await Client.GetStream().ReadAsync(buffer, 0, 10360);

                    
                    if (nextMessageChatMessage)
                    {
                        nextMessageChatMessage = false;
                        ChatMessage chatMessage = DeserializeChatMessage(buffer);

                        lbxChat.Items.Add(DateTime.Now + " " + chatMessage.FromUser + ": " + chatMessage.Message);
                    }
                    else if (nextMessageUserList)
                    {
                        nextMessageUserList = false;
                        userList = DeserializeUserList(buffer);

                        if (!alreadyConnected)
                        {
                            lbxChat.Items.Add(DateTime.Now + ": Connected to server. Say hello!");
                            btnLeave.Enabled = true;
                            alreadyConnected = true;
                        }
                        else
                        {
                            if (lbxUsers.Items.Count > userList.Count)
                            {
                                string[] tmpUsers = new string[userList.Count];
                                for (int i = 0; i < userList.Count; i++)
                                {
                                    tmpUsers[i] = userList[i].ClientNickname;
                                }

                                for (int i = 0; i < lbxUsers.Items.Count; i++)
                                {
                                    if (!tmpUsers.Contains(lbxUsers.Items[i]))
                                    {
                                        lbxChat.Items.Add(DateTime.Now + ": " + lbxUsers.Items[i] + " disconnected from the server.");
                                    }
                                }
                            }
                            for (int i = 0; i < userList.Count; i++)
                            {
                                if (!lbxUsers.Items.Contains(userList[i].ClientNickname))
                                {
                                    lbxChat.Items.Add(DateTime.Now + ": " + userList[i].ClientNickname + " connected to the server. Say hello!");
                                }
                            }
                            
                            btnLeave.Enabled = true;
                            alreadyConnected = true;
                        }

                        updateUserList();
                    }
                    else
                    {
                        message = Encoding.UTF8.GetString(buffer, 0, n);
                    }

                    if (message.Equals("R-A")) //Server is requesting authentication
                    {
                        SendAuth();
                    }
                    else if (message.Equals("U-L-98759183")) //UserList coming in next message
                    {
                        nextMessageUserList = true;
                    }
                    else if (message.Equals("C-M")) //Server is sending a chat message in the next message
                    {
                        nextMessageChatMessage = true;
                    }
                    else if (message.Equals("K-D")) //Server has sent a kick request to the client. Connection will be terminated.
                    {
                        Client.Close();
                        MessageBox.Show(this, "You have been kicked from this chat-server by an administrator. Please adhere to the rules of the server and contact an administrator if you believe this action to have been wrongly performed.", "You have been Kicked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (message.Equals("C-C")) //Server is force-clearing all messages from the chat.
                    {
                        lbxChat.Items.Clear();
                        MessageBox.Show(this, "The chat has been force-cleared by the server.", "Chat Force-Cleared by Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (message.Equals("S-C")) //Server is closing. Prepare for session close.
                    {
                        Client.Close();
                        MessageBox.Show(this, "This server has been closed. Please try to log in later if this is an unexpected event.", "Server Closed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception e)
                {
                    if (Client.Connected)
                    {
                        errorHandle(e);
                    }
                }


                StartReceiving();
            }
        }

        private async void SendAuth()
        {
            string message = "Auth: Nickname: " + tbxUsername.Text;
            tbxUsername.Enabled = false;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
        }

        private async void SendDisconnect()
        {
            string message = "Disconnecting";
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);

            try
            {
                if (Client.Connected)
                {
                    Client.Close();
                }
                else
                {
                    Client.Dispose();
                }
            }
            catch
            {
                //Do nothing, client is already disposed and closed. Error is wrongly generated by TCPClient Class
            }

            lbxChat.Items.Add(DateTime.Now + ": Disconnected from server");
            lbxUsers.Items.Clear();
            btnJoin.Enabled = true;
            btnLeave.Enabled = false;
            tbxUsername.Enabled = true;
            alreadyConnected = false;
        }

        private async void SendMessage(string message)
        {
            message = "C-M" + message;
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
        }


        private static List<CB_Simulator_clientInfoLight> DeserializeUserList (byte[] userList)
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

        private static ChatMessage DeserializeChatMessage (byte[] message)
        {
            string fromUser = Encoding.UTF8.GetString(message, 0, 512);
            fromUser = fromUser.Substring(0, Math.Max(0, fromUser.IndexOf('\0')));

            string sentMessage = Encoding.UTF8.GetString(message, 512, 2048);
            sentMessage = sentMessage.Substring(0, Math.Max(0, sentMessage.IndexOf('\0')));

            ChatMessage tmpChatMessage = new ChatMessage(fromUser, sentMessage);

            return tmpChatMessage;
        }

        private void updateUserList()
        {
            lbxUsers.Items.Clear();
            for (int i = 0; i < userList.Count; i++)
            {
                lbxUsers.Items.Add(userList[i].ClientNickname);
            }
        }

        private void errorHandle(Exception e)
        {
            MessageBox.Show(this, e.Message, "An error has occured in the client", MessageBoxButtons.OK);
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxUsername.Text) && !string.IsNullOrWhiteSpace(tbxUsername.Text))
            {
                startBroadcastListening();

                isBroadcastReceivedTimer = new Timer();
                isBroadcastReceivedTimer.Tick += new EventHandler(isBroadcastReceived);
                isBroadcastReceivedTimer.Interval = 1000; // in miliseconds
                isBroadcastReceivedTimer.Start();

                btnJoin.Enabled = false;
            }
            else
            {
                MessageBox.Show(this, "Please enter a valid username", "Invalid Username", MessageBoxButtons.OK);
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            SendDisconnect();
        }

        private void btnClearChat_Click(object sender, EventArgs e)
        {
            lbxChat.Items.Clear();
            lbxChat.Items.Add("Observe that the chat has only been cleared for this client, and not other clients connected to the same server.");
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxMessage.Text) && !String.IsNullOrWhiteSpace(tbxMessage.Text))
            {
                lbxChat.Items.Add(DateTime.Now + " " + tbxUsername.Text + ": " + tbxMessage.Text);
                SendMessage(tbxMessage.Text);
                tbxMessage.Text = "";
            }
        }
    }
}
