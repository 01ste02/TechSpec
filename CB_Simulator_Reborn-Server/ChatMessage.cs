/* Copyright (C) StenIT - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Oskar Stenberg <oskar@stenit.eu>, January-February 2020
 */


using System.Text;

namespace CB_Simulator_Reborn_Server
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

        public byte[] ToByteArray()
        {
            byte[] tmp = new byte[512 + 2048]; //Make an array with 512 bytes reserved for username, and 2048 for the message
            byte[] messageBytes = new byte[2048];

            Encoding.UTF8.GetBytes(message).CopyTo(messageBytes, 0); //Convert the message to a byte array

            Encoding.UTF8.GetBytes(fromUser).CopyTo(tmp, 0); //Copy the username to the byte array to be returned
            messageBytes.CopyTo(tmp, 512); //Add the message byte array after the username

            return tmp;
        }
    }
}
