/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */


namespace CB_Simulator_Reborn_Client
{
    public class ChatMessage
    {
        private string fromUser;
        private string message;

        public ChatMessage(string fromUser, string message)
        {
            FromUser = fromUser;
            Message = message;
        }

        public string FromUser
        {
            get
            {
                return fromUser;
            }
            set
            {
                fromUser = value;
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
    }
}
