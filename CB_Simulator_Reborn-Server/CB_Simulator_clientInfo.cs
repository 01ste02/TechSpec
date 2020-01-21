using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CB_Simulator_Reborn_Server
{
    class CB_Simulator_clientInfo
    {
        private IPAddress ip;
        private int id;
        private DateTime connectTime;
        private string nickname;

        public CB_Simulator_clientInfo(IPAddress ip, int id, DateTime connectTime, string nickname)
        {
            ClientIP = ip;
            ClientId = id;
            ClientConnectTime = connectTime;
            ClientNickname = nickname;
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
