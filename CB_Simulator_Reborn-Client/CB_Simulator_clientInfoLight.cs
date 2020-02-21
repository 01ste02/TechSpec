/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */



namespace CB_Simulator_Reborn_Client
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
    }
}
