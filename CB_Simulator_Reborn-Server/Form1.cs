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
        int serverPort = 11563;
        IPAddress serverIP = IPAddress.Any;
        UdpClient broadcaster = new UdpClient();
        const int broadcastPort = 15000;
        IPEndPoint broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, broadcastPort);
        string broadcastMessage = "Server is on: 11563";
        DateTime lastBroadcastTime = DateTime.Now;

        public Form1()
        {
            InitializeComponent();
        }

        public async void Broadcaster()
        {
            if (0 <= DateTime.Compare(DateTime.Now, lastBroadcastTime.AddSeconds(30)))
            {
                broadcaster.EnableBroadcast = true;
                byte[] broadcastBytes = Encoding.UTF8.GetBytes(broadcastMessage);
                await broadcaster.SendAsync(broadcastBytes, broadcastBytes.Length, broadcastEndpoint);
                lastBroadcastTime = DateTime.Now;
            }
            Broadcaster();
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
