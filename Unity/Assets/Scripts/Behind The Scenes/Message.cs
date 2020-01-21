using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Behind_The_Scenes
{
    [Serializable]
    public class Message
    {
        static string[] URGENCY_STATUS = { "Normal", "Expedited", "Urgent" };
        

        // For debugging purposes
        string filepath;

        public Message(int i, string r, string s, string m, int u, bool d, bool h)
        {
            MessageID = i;
            Recipient = r;
            Sender = s;
            MessageBody = m;
            UrgencyIndex = u;
            HasBeenDelivered = d;
        }

        // Deep Copy Constructor
        public Message(Message m)
        {
            MessageID = m.MessageID;
            Recipient = m.Recipient;
            Sender = m.Sender;
            MessageBody = m.MessageBody;
            UrgencyIndex = m.UrgencyIndex;
            HasBeenDelivered = m.HasBeenDelivered;
        }
        
        public int MessageID { get; private set; }

        public string Recipient { get; private set; }

        public string Sender { get; private set; }

        public string MessageBody { get; private set; }

        public string Urgency
        {
            get
            {
                return URGENCY_STATUS[UrgencyIndex];
            }
        }

        public int UrgencyIndex { get; private set; }

        public bool HasBeenDelivered { get; set; }

        public bool IsOnHold { get; set; }

        /// <summary>
        /// Helps parse the contents read in from the file into a Message object
        /// </summary>
        /// <param name="r">Recipient of the message</param>
        /// <param name="s">Sender of the message</param>
        /// <param name="m">Message body</param>
        /// <param name="u">Urgency</param>
        /// <returns></returns>
        public static Message ParseMessage(int id, string r, string s, string m, string u)
        {
            string[] tempParts;
            
            tempParts = r.Split(':');
            string recipient = tempParts[1].Trim();

            tempParts = s.Split(':');
            string sender = tempParts[1].Trim();

            string message = m;

            tempParts = u.Split(':');
            string urgency = tempParts[1].Trim();
            
            int urgencyIndex = 0;

            // Find urgency index
            for (int i = 0; i < URGENCY_STATUS.Length; i++)
            {
                if (URGENCY_STATUS[i] == urgency)
                {
                    urgencyIndex = i;
                    break;
                }
            }
            
            return new Message(id, recipient, sender, message, urgencyIndex, false, false);
        }

        /// <summary>
        /// Helps parse the contents read in from the file into a Message object
        /// </summary>
        /// <param name="r">Recipient of the message</param>
        /// <param name="s">Sender of the message</param>
        /// <param name="m">Message body</param>
        /// <param name="u">Urgency</param>
        /// <param name="d">Indicates if the message was delivered</param>
        /// <returns></returns>
        public static Message ParseMessage(int id, string r, string s, string m, string u, bool d, bool h)
        {
            Message thisMessage = ParseMessage(id, r, s, m, u);
            thisMessage.HasBeenDelivered = d;
            thisMessage.IsOnHold = h;

            return thisMessage;
        }
    }
}
