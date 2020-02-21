/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */





using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace CB_Simulator_Reborn_Server
{
    public partial class CB_Simulator_Reborn_Server : Form
    {
        //Broadcast variables, server broadcasts its port so this can change and the clients don't need a hard-coded port. 
        //CB-Simulator was for local networks when it first came out, so a broadcast is suitable for this application.
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

        string authRequestMessage = "R-A"; //Message for the server to send the client to let it know to send the username (and maybe password)

        public CB_Simulator_Reborn_Server()
        {
            InitializeComponent();
        }

        public void InitBroadcastTimer() //Start the timer to broadcast the server port once per second. Could be sped up, but kept here as not to flood the network with broadcasts.
        {
            broadcastTimer = new Timer();
            broadcastTimer.Tick += new EventHandler(broadcastTimer_Tick);
            broadcastTimer.Interval = 1000; // in miliseconds
            broadcastTimer.Start();
        }

        private void broadcastTimer_Tick(object sender, EventArgs e)
        {
            Broadcaster(); //Event handler didn't like being async, so this calls the async broadcast method.
        }

        public async void Broadcaster()
        {
            try
            {
                broadcaster.EnableBroadcast = true; //UDP clients have to have this enable to listen to or to send broadcasts
                byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastMessage);
                await broadcaster.SendAsync(broadcastBytes, broadcastBytes.Length, broadcastEndpoint);
            }
            catch (Exception e)
            {
                if (broadcaster.EnableBroadcast == true) //The await generates an error if the broadcast is stopped (+disabled) when it is in the middle of transmitting. Don't throw the error unless it is an unexpected, actual error.
                {
                    errorHandle(e);
                }
            }
        }

        public void StartListening()
        {
            try
            {
                serverListener = new TcpListener(IPAddress.Any, serverPort); //Start the server listener and start accepting clients.
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

                if (broadcaster.EnableBroadcast == true)
                {
                    AcceptClients(); //Don't try to accept new clients unless the server is broadcasting to them.
                }
            }
            catch (Exception e)
            {
                if (broadcaster.EnableBroadcast == true) //If this is false, the server shouldn't be accepting clients. Error generated in fault due to timing of stop.
                {
                    errorHandle(e);
                }
            }
        }

        public async void SendAuthRequest(TcpClient client)
        {
            try
            {
                byte[] message = Encoding.UTF8.GetBytes(authRequestMessage); //If a client has tried to connect, tell it to authenticate with username (and password in the future)
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
                n = await client.GetStream().ReadAsync(buffer, 0, 1024); //Get the auth reply from the client
                message = Encoding.UTF8.GetString(buffer, 0, n);
                string nickname = message.Substring(15); //Cut out the auth header

                IPEndPoint tmpEndpoint = client.Client.LocalEndPoint as IPEndPoint;
                CB_Simulator_clientInfo tmpClient = new CB_Simulator_clientInfo(client, tmpEndpoint.Address, clientList.Count + 1, DateTime.Now, nickname); //Create all the nessecary records from the client
                CB_Simulator_clientInfoLight tmpClientLight = new CB_Simulator_clientInfoLight(clientList.Count + 1, nickname); //Create a light record, to transmit in a user list in the future (unsafe to broadcast all info)
                clientList.Add(tmpClient);
                clientListLight.Add(tmpClientLight); //Add the client to the record tables
                Console.WriteLine("Added new client");

                lbxConsole.Items.Add(DateTime.Now + ": New client connected from " + tmpClient.ClientIP + " with the username " + tmpClient.ClientNickname);

                for (int m = 0; m < clientList.Count; m++)
                {
                    SendUserList(clientList[m].Client); //Tell all currently connected clients which users are online and connected
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
                byte[] messageHeader = Encoding.UTF8.GetBytes("U-L-98759183"); //Prepare the client for an incoming user-list

                await client.GetStream().WriteAsync(messageHeader, 0, messageHeader.Length);

                updateUserList(); //Update the userList in the server GUI, could be done outside this function, but added here to minimize code. Performance impact not notable.

                Timer userListDelayTimer = new Timer(); //Broadcast the user list in 500 mSeconds when the client is ready. Keep as short as possible as to assure that the client is still waiting for the list.
                userListDelayTimer.Tick += (sender, e) => SendUserListSerialized(sender, e, client);
                userListDelayTimer.Interval = 500; // in miliseconds
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
                Timer tmp = sender as Timer; //Function triggered, timer no longer needed since it only needs to run one time.
                tmp.Stop();
                

                byte[] message = SerializeUserList(clientListLight); //Serialize the user list to a byte array and send it.
                await client.GetStream().WriteAsync(message, 0, message.Length);
                ReceiveFromClient(client); //Start accepting data from the client again
            }
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private void updateUserList()
        {
            lbxUsers.Items.Clear(); //Clear the list and refill it with the latest user list
            for (int i = 0; i < clientList.Count; i++)
            {
                lbxUsers.Items.Add(clientList[i].ClientNickname);
            }
        }

        private static byte[] SerializeUserList(List<CB_Simulator_clientInfoLight> userList)
        {
            byte[] tmp = new byte[userList.Count * (512 + 4) + 4]; //Create a byte array large enough to contain all the users, fill it, and return it
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
                n = await client.GetStream().ReadAsync(buffer, 0, 10360); //Very large buffer to account for large messages and future functions.
                message = Encoding.UTF8.GetString(buffer, 0, n);
                
                if (message.Equals("Disconnecting")) //If a client is going to disconnect, remove it from the user lists, tell the other clients, and close the connection gracefully
                {
                    IPEndPoint tmpEndpoint = client.Client.LocalEndPoint as IPEndPoint;
                    for (int i = 0; i < clientList.Count; i++) //Find where the current client is in the lists, remove them, and update the others
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
                else if (message.Substring(0, 3).Equals("C-M"))  //This header indicates that this is a new chat-message from a client
                {
                    ChatMessage chatMessage = new ChatMessage("None", "None");

                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].Client == client)
                        {
                            chatMessage = new ChatMessage(clientList[i].ClientNickname, message.Substring(3)); //Find the username of the client who sent the message, and update the chatMessage object
                            break;
                        }
                    }

                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].Client != client) //Forward the chat message to all clients except for the one who sent it at first
                        {
                            ForwardMessageToClients(clientList[i].Client, chatMessage); 
                        }
                    }

                    lbxConsole.Items.Add(DateTime.Now + " " + chatMessage.FromUser + ": " + chatMessage.Message);
                }

                if (client.Connected) //If this is true, it means that the client wasn't disconnecting, so we should expect more data from it
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
                byte[] messageHeader = Encoding.UTF8.GetBytes("C-M"); //Prepare the client that the server is forwarding a chat message
                await client.GetStream().WriteAsync(messageHeader, 0, messageHeader.Length);

                Timer forwardMessageTimer = new Timer(); //Send the message when the client is listening. Explained in greater detail in the SendUserList function
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
                Timer tmp = sender as Timer; //Almost the same as the SendUserListSerialized function, variables modified for sending chat messages.
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            InitBroadcastTimer(); //Start the broadcast, accept new clients and enable all the functions of the program
            StartListening();

            btnStopServer.Enabled = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnClearAll.Enabled = true;
            btnClearConsole.Enabled = true;
            btnKick.Enabled = true;
            btnSendMessage.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            broadcastTimer.Stop(); //Stop accepting new clients, but currently connected clients may stay connected
            serverListener.Stop();
            broadcaster.EnableBroadcast = false;

            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        private async void btnKick_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxUsers.SelectedIndex >= 0)
                {
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        if (clientList[i].ClientNickname.Equals(lbxUsers.Items[lbxUsers.SelectedIndex])) //Find which client is selected for kicking
                        {
                            TcpClient client = clientList[i].Client;

                            byte[] message = Encoding.UTF8.GetBytes("K-D"); //Tell the client that they are getting kicked
                            await client.GetStream().WriteAsync(message, 0, message.Length);

                            client.Close(); //Close the connection, remove the client from the user-lists, and tell the others that the user has disconnected

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
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private async void btnStopServer_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < clientList.Count; i++) //Tell all clients that the server is closing, so they should disconnect
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

                btnStopServer.Enabled = false; //Disable the server entirely
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                btnClearAll.Enabled = false;
                btnClearConsole.Enabled = true;
                btnKick.Enabled = false;
                btnSendMessage.Enabled = false;

                MessageBox.Show(this, "The server-close signal has been sent", "Server Close", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private void btnClearConsole_Click(object sender, EventArgs e)
        {
            lbxConsole.Items.Clear();
            lbxConsole.Items.Add("Observe that the console has only been cleared for the server, and not the clients. Use the \"Clear All\" button to clear the chat for all clients.");
        }

        private async void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < clientList.Count; i++) //Tell all clients to clear the chats. Useful if a message needs to be moderated.
                {
                    TcpClient client = clientList[i].Client;

                    byte[] message = Encoding.UTF8.GetBytes("C-C");
                    await client.GetStream().WriteAsync(message, 0, message.Length);
                }

                lbxConsole.Items.Add(DateTime.Now + ": All client chats have been cleared");
                MessageBox.Show(this, "All chats have been force-cleared.", "Chat Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private async void btnSaveLog_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = saveConsoleDialog.ShowDialog(); //Server operations are paused while this dialogue is open. Expected usage is only when server is stopped, or in extreme cases where the logs need to be saved due to the nature of conversation. Performance impact is major, but should not affect normal operations.

                if (result == DialogResult.OK)
                {
                    FileStream outStream = new FileStream(saveConsoleDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter writer = new StreamWriter(outStream, Encoding.UTF8);

                    for (int i = 0; i < lbxConsole.Items.Count; i++) //Find where to save the file, and write each line from the console to it.
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
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(tbxMessage.Text) && !String.IsNullOrWhiteSpace(tbxMessage.Text)) //Unless the message is empty or too long, send a message to all clients from the server
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
            catch (Exception e2)
            {
                errorHandle(e2);
            }
        }

        private void formClosing(object sender, FormClosedEventArgs e)
        {
            btnStopServer_Click(this, e);
        }

        public void errorHandle(Exception e)
        {
            MessageBox.Show(this, e.Message, "An error has occured in the server", MessageBoxButtons.OK, MessageBoxIcon.Warning); //Tell the user that an error has occured, with a brief error description for the support team
        }
    }
}
