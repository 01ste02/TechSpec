/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */





using System;
using System.Net;
using System.Net.Sockets;

namespace CB_Simulator_Reborn_Client
{
    [SerializableAttribute]
    public class CB_Simulator_clientInfo
    {
        private TcpClient client;
        private IPAddress ip;
        private int id;
        private DateTime connectTime;
        private string nickname;

        public CB_Simulator_clientInfo(TcpClient client, IPAddress ip, int id, DateTime connectTime, string nickname)
        {
            Client = client;
            ClientIP = ip;
            ClientId = id;
            ClientConnectTime = connectTime;
            ClientNickname = nickname;
        }

        public TcpClient Client
        {
            get
            {
                return client;
            }
            set
            {
                try
                {
                    client = value;
                }
                catch
                {

                }
            }
        }

        public IPAddress ClientIP
        {
            get
            {
                return ip;
            }
            set
            {
                try
                {
                    ip = value;
                }
                catch
                {

                }
            }
        }

        public int ClientId
        {
            get
            {
                return id;
            }
            set
            {
                try
                {
                    id = value;
                }
                catch
                {

                }
            }
        }

        public DateTime ClientConnectTime
        {
            get
            {
                return connectTime;
            }
            set
            {
                try
                {
                    connectTime = value;
                }
                catch
                {

                }
            }
        }

        public string ClientNickname
        {
            get
            {
                return nickname;
            }
            set
            {
                try
                {
                    nickname = value;
                }
                catch
                {

                }
            }
        }
    }
}