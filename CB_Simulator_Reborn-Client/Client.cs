/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */





using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using PopUp;

namespace CB_Simulator_Reborn_Client
{
    public partial class CB_Simulator_Reborn_Client : Form
    {
        private const int broadcastPort = 15000; //Variables used to find the server port
        private UdpClient broadcastReceiver;
        private IPAddress serverIP;
        private int serverPort;
        private bool broadcastReceived = false;
        private Timer isBroadcastReceivedTimer;
        private int secondsSinceConnectAttempt = 0; //For the time-out-warning on trying to connect

        private bool nextMessageUserList = false; //Variables for receiving messages
        private bool nextMessageChatMessage = false;
        private bool alreadyConnected = false;
        private List<CB_Simulator_clientInfoLight> userList;

        private TcpClient Client;

        private static TimedPopUp asyncPopUp = new TimedPopUp(); //Async messageBox

        public CB_Simulator_Reborn_Client()
        {
            InitializeComponent();
        }

        private void StartBroadcastListening() //Start listening for which port the server is on. Call receiveBroadcast when a broadcast is received
        {
            try
            {
                broadcastReceiver = new UdpClient(broadcastPort);
                broadcastReceiver.EnableBroadcast = true;
                broadcastReceiver.BeginReceive(ReceiveBroadcast, new Object());
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        private void ReceiveBroadcast (IAsyncResult result)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, broadcastPort); //Store the IP of the server from the broadcast
                byte[] broadcastBytes = broadcastReceiver.EndReceive(result, ref ip);
                String broadcastString = Encoding.UTF8.GetString(broadcastBytes); //Extract the string that was broadcasted
                serverIP = ip.Address;
                serverPort = int.Parse(broadcastString.Substring(14)); //Remove the broadcast header, and the port should be found
                Console.WriteLine("Broadcast Received: " + broadcastString + " " + serverIP.ToString());

                broadcastReceiver.Dispose(); //Disable the broadcast and let the rest of the client know that a broadcast has been received
                broadcastReceived = true;
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        private void IsBroadcastReceived (object sender, EventArgs e)
        {
            if (broadcastReceived) //Function is triggered by a periodic timer. If the broadcast is received, connect to the server and stop the timer
            {
                ConnectToServer();
                isBroadcastReceivedTimer.Stop();
                secondsSinceConnectAttempt = 0;
            }
            else
            {
                secondsSinceConnectAttempt++;

                if (secondsSinceConnectAttempt >= 3)
                {
                    //MessageBox.Show(this, "Connecting to the server is taking longer than expected. Make sure that you are connected to the same network as the server, and that your firewall is not blocking the client or the server. The server might have joining turned off, or be turned off. Try again later, or after checking afforementioned factors.", "Delay in Joining", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    asyncPopUp.Set("Connecting to the server is taking longer than expected. Make sure that you are connected to the same network as the server, and that your firewall is not blocking the client or the server. The server might have joining turned off, or be turned off. Try again later, or after checking afforementioned factors.", "Delay in Joining", 5000);
                    asyncPopUp.Show();

                    secondsSinceConnectAttempt = -7;
                    btnLeave.Enabled = true;
                }
            }
        }

        private async void ConnectToServer()
        {
            try
            {
                Client = new TcpClient(); //Start the tcp client, connect, and wait for the server to request authentication from the client
                await Client.ConnectAsync(serverIP, serverPort);
                StartReceiving();
                btnSendMessage.Enabled = true;
            }
            catch (Exception e)
            {
                if (Client.Connected) //If the client is not connected, the error was generated due to trying to listen when kicked/disconnected/when the server is closing. Error generated due to a bug that is out of developer control, therefore it is suppressed
                {
                    ErrorHandle(e);
                }
            }
        }


        private async void StartReceiving ()
        {
            if (Client.Connected)
            {
                byte[] buffer = new byte[10360]; //VERY large buffer, to accomodate for future functionality. Performance impact should be minimal

                int n = 0;
                string message = "";

                try
                {
                    n = await Client.GetStream().ReadAsync(buffer, 0, 10360);


                    if (nextMessageChatMessage) //This is true if the server has announced that the next message will be a forwarded chat message
                    {
                        nextMessageChatMessage = false;
                        ChatMessage chatMessage = DeserializeChatMessage(buffer); //Deserialize the buffer to extract the message data, and add the message to the chat

                        lbxChat.Items.Add(DateTime.Now + " " + chatMessage.FromUser + ": " + chatMessage.Message);
                    }
                    else if (nextMessageUserList) //True if server has announced that the user list is coming in the next message
                    {
                        nextMessageUserList = false;
                        userList = DeserializeUserList(buffer); //Deserialize the buffer into the userList

                        if (!alreadyConnected) //If the client wasn't connected, we are now. Tell the user so, and update the variables.
                        {
                            lbxChat.Items.Add(DateTime.Now + ": Connected to server. Say hello!");
                            btnLeave.Enabled = true;
                            alreadyConnected = true;
                        }
                        else //If the client was connected, check if the list has updated
                        {
                            if (lbxUsers.Items.Count > userList.Count) //If the list is shorter, someone disconnected
                            {
                                string[] tmpUsers = new string[userList.Count];
                                for (int i = 0; i < userList.Count; i++) //Make a new, temporary array containing less data (for faster analysis), and fill it with the usernames that were just received
                                {
                                    tmpUsers[i] = userList[i].ClientNickname;
                                }

                                for (int i = 0; i < lbxUsers.Items.Count; i++)
                                {
                                    if (!tmpUsers.Contains(lbxUsers.Items[i])) //Check which user/what users have disconnected, and announce that to the user
                                    {
                                        lbxChat.Items.Add(DateTime.Now + ": " + lbxUsers.Items[i] + " disconnected from the server.");
                                    }
                                }
                            }
                            for (int i = 0; i < userList.Count; i++)
                            {
                                if (!lbxUsers.Items.Contains(userList[i].ClientNickname)) //Check if there are any new users (who were not previously in the userList), and announce their prescence to the user
                                {
                                    lbxChat.Items.Add(DateTime.Now + ": " + userList[i].ClientNickname + " connected to the server. Say hello!");
                                }
                            }

                            btnLeave.Enabled = true;
                            alreadyConnected = true;
                        }

                        UpdateUserList(); //Update the list in the GUI
                    }
                    else
                    {
                        message = Encoding.UTF8.GetString(buffer, 0, n); //Treat the next message as a pure string. Could contain a request from the server
                    }

                    if (message.Equals("R-A")) //Server is requesting authentication, send it
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
                        Client.Close(); //Close the connection from our side, and announce it to the user. Make the user able to join a new server

                        lbxChat.Items.Add(DateTime.Now + ": Kicked from server");
                        lbxUsers.Items.Clear();
                        btnJoin.Enabled = true;
                        btnLeave.Enabled = false;
                        tbxUsername.Enabled = true;
                        alreadyConnected = false;
                        btnSendMessage.Enabled = false;

                        //MessageBox.Show(this, "You have been kicked from this chat-server by an administrator. Please adhere to the rules of the server and contact an administrator if you believe this action to have been wrongly performed.", "You have been Kicked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        asyncPopUp.Set("You have been kicked from this chat-server by an administrator. Please adhere to the rules of the server and contact an administrator if you believe this action to have been wrongly performed.", "You have been Kicked", 5000);
                        asyncPopUp.Show();
                    }
                    else if (message.Equals("C-C")) //Server is force-clearing all messages from the chat.
                    {
                        lbxChat.Items.Clear();
                        lbxChat.Items.Add(DateTime.Now + ": Chat cleared by the server");
                        //MessageBox.Show(this, "The chat has been force-cleared by the server.", "Chat Force-Cleared by Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        asyncPopUp.Set("The chat has been force-cleared by the server.", "Chat Force-Cleared by Server", 5000);
                        asyncPopUp.Show();
                    }
                    else if (message.Equals("S-C")) //Server is closing. Prepare for session close.
                    {
                        Client.Close(); //Closing our connection so it isn't forcefully terminated

                        lbxChat.Items.Add(DateTime.Now + ": Server closed");
                        lbxUsers.Items.Clear();
                        btnJoin.Enabled = true;
                        btnLeave.Enabled = false;
                        tbxUsername.Enabled = true;
                        alreadyConnected = false;
                        btnSendMessage.Enabled = false;

                        //MessageBox.Show(this, "This server has been closed. Please try to log in later if this is an unexpected event.", "Server Closed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        asyncPopUp.Set("This server has been closed. Please try to log in later if this is an unexpected event.", "Server Closed", 5000);
                        asyncPopUp.Show();
                    }
                }
                catch (Exception e)
                {
                    if (Client.Connected) //If the client isn't connected, this is an error generated due to the exact timing that the disconnect was triggered at, and should be disregarded as such
                    {
                        ErrorHandle(e);
                    }
                }

                StartReceiving(); //Client needs to listen to new messages from the server at all times. If-statement in the beginning of the function checks if the client should proceed, or if it has been disconnected
            }
        }

        private async void SendAuth()
        {
            try
            {
                string message = "Auth: Nickname: " + tbxUsername.Text;
                tbxUsername.Enabled = false;
                byte[] messageBytes = Encoding.UTF8.GetBytes(message); //User will not be able to change username without logging out first. Send the username with a request header. Max length of username set in the form itself
                await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }

        private async void SendDisconnect()
        {
            try
            {
                string message = "Disconnecting";
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length); //Tell the server that the client is disconnecting.

                broadcastReceiver.EnableBroadcast = false;
                if (Client.Connected) //If the client is connected, disconnect. Server could have disconnected the client before this runs
                {
                    Client.Close();
                }
                else
                {
                    Client.Dispose(); //Added due to an error in the current version of the TcpClient class that doesn't dispose of the client if the connection is remotely closed
                }
            }
            catch
            {
                //Do nothing, client is already disposed and closed. Error is wrongly generated by TCPClient Class as explained above
            }

            lbxChat.Items.Add(DateTime.Now + ": Disconnected from server"); //Announce this to the user, and update the neccesary buttons and variables
            lbxUsers.Items.Clear();
            btnJoin.Enabled = true;
            btnLeave.Enabled = false;
            tbxUsername.Enabled = true;
            alreadyConnected = false;
        }

        private async void SendMessage(string message)
        {
            try
            {
                message = "C-M" + message; //Send a chat-message with the C-M header to indicate the message type
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await Client.GetStream().WriteAsync(messageBytes, 0, messageBytes.Length);
            }
            catch (Exception e)
            {
                ErrorHandle(e);
            }
        }


        private static List<CB_Simulator_clientInfoLight> DeserializeUserList (byte[] userList)
        {
            List<CB_Simulator_clientInfoLight> tmpUserList = new List<CB_Simulator_clientInfoLight>(); //Make a temporary list
            try
            {
                int index = 0;

                int userCount = BitConverter.ToInt32(userList, 0); //Find the amount of elements based on the first int in the byte array
                while (index < userCount)
                {
                    int userId = BitConverter.ToInt32(userList, 4 + (4 + 512) * index);
                    string username = Encoding.UTF8.GetString(userList, 8 + (4 + 512) * index, 512);

                    username = username.Substring(0, Math.Max(0, username.IndexOf('\0')));

                    CB_Simulator_clientInfoLight tmpClient = new CB_Simulator_clientInfoLight(userId, username);
                    tmpUserList.Add(tmpClient);
                    index++;

                }
            }
            catch
            { //Do not do anything, since the list will be re-sent soon enough. Log for developers, but return an empty list.
                Console.WriteLine("Error in deserializing user-list");
            }

            return tmpUserList;
        }

        private static ChatMessage DeserializeChatMessage (byte[] message)
        {
            ChatMessage tmpChatMessage = new ChatMessage("Error", "Error"); //Default message
            try
            {
                string fromUser = Encoding.UTF8.GetString(message, 0, 512); //Get the username of the user who sent the message
                fromUser = fromUser.Substring(0, Math.Max(0, fromUser.IndexOf('\0'))); //Remove trailing whitespaces

                string sentMessage = Encoding.UTF8.GetString(message, 512, 2048); //The rest of the string is the message. Decode it
                sentMessage = sentMessage.Substring(0, Math.Max(0, sentMessage.IndexOf('\0'))); //Remove trailing whitespaces

                tmpChatMessage = new ChatMessage(fromUser, sentMessage); //Create an object for the message, and return it
            }
            catch
            { //Do not do anything. Message is not decryptable (corrupt), and is non-recoverable. Log for developers. Default message indicates error for the user
                Console.WriteLine("Error in deserializing chat-message");
            }

            return tmpChatMessage;
        }

        private void UpdateUserList()
        {
            lbxUsers.Items.Clear(); //Clear the list and add the users from the latest list
            for (int i = 0; i < userList.Count; i++)
            {
                lbxUsers.Items.Add(userList[i].ClientNickname);
            }
        }

        private void BtnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbxUsername.Text) && !string.IsNullOrWhiteSpace(tbxUsername.Text)) //Do not allow user to join with an empty username. Name length is limited by the control itself
                {
                    StartBroadcastListening(); //Start listening for the server port

                    isBroadcastReceivedTimer = new Timer();
                    isBroadcastReceivedTimer.Tick += new EventHandler(IsBroadcastReceived);
                    isBroadcastReceivedTimer.Interval = 1000; // in miliseconds
                    isBroadcastReceivedTimer.Start(); //Start checking if the server port has been received, check once per second

                    btnJoin.Enabled = false; //Currently joining. Disable the ability to join again
                }
                else
                {
                    //MessageBox.Show(this, "Please enter a valid username", "Invalid Username", MessageBoxButtons.OK);
                    asyncPopUp.Set("Please enter a valid username", "Invalid Username", 5000);
                    asyncPopUp.Show();
                }
            }
            catch (Exception e2)
            {
                ErrorHandle(e2);
            }
        }

        private void BtnLeave_Click(object sender, EventArgs e)
        {
            SendDisconnect(); //Leave the server, and tell the server beforehand

            if (isBroadcastReceivedTimer.Enabled) //If the client is trying to connect, and the leave button has been pressed after a timeout event, stop the times as well (stop trying to connect)
            {
                isBroadcastReceivedTimer.Stop();
            }
        }

        private void BtnClearChat_Click(object sender, EventArgs e)
        {
            lbxChat.Items.Clear();
            lbxChat.Items.Add("Observe that the chat has only been cleared for this client, and not other clients connected to the same server.");
        }

        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(tbxMessage.Text) && !String.IsNullOrWhiteSpace(tbxMessage.Text)) //Do not send empty messages. Assume missclick
                {
                    if (tbxMessage.Text.Length < 1024) //Messages can't be too long. No longer than 2048 bytes, which means 1024 characters
                    {
                        lbxChat.Items.Add(DateTime.Now + " " + tbxUsername.Text + ": " + tbxMessage.Text); //Add the message to the local console, and send the message. Erase the message from the chatbox after sending
                        SendMessage(tbxMessage.Text);
                        tbxMessage.Text = "";
                    }
                    else
                    {
                        //MessageBox.Show(this, "Your entered message is too long", "Message too long", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        asyncPopUp.Set("The message that you entered is too long", "Message too long", 5000);
                        asyncPopUp.Show();
                    }
                }
            }
            catch (Exception e2)
            {
                ErrorHandle(e2);
            }
        }

        private void ErrorHandle(Exception e)
        {
            //MessageBox.Show(this, e.Message, "An error has occured in the client", MessageBoxButtons.OK);
            asyncPopUp.Set(e.Message + " " + e.TargetSite + " " + e.StackTrace, "An error has occured in the client", 10000);
            asyncPopUp.Show();
        }
    }
}
