using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
