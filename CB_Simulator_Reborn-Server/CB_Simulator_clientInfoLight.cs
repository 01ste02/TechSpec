using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientListNamespace2
{
    [System.Serializable]
    public class CB_Simulator_clientInfoLight
    {
        public Int32 x;
        private int id;
        private DateTime connectTime;
        private string nickname;

        public CB_Simulator_clientInfoLight(int id = 99999, DateTime connectTime = default(DateTime), string nickname = "NoNickname")
        {
            ClientId = id;
            ClientConnectTime = connectTime;
            ClientNickname = nickname;
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
