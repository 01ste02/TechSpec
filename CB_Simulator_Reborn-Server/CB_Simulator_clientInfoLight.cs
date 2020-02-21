/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */




using System;
using System.Text;

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
