﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CB_Simulator_Reborn_Server
{
    public class CB_Simulator_clientInfoLight
    {
        private int id;
        private string nickname;

        public CB_Simulator_clientInfoLight(int id, string nickname)
        {
            ClientId = id;
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

        public byte[] ToByteArray ()
        {
            byte[] tmp = new byte[4 + 512];
            byte[] nicknameBytes = new byte[512];

            Encoding.UTF8.GetBytes(nickname).CopyTo(nicknameBytes, 0);

            BitConverter.GetBytes(id).CopyTo(tmp, 0);
            nicknameBytes.CopyTo(tmp, 4);

            return tmp;
        }
    }
}
