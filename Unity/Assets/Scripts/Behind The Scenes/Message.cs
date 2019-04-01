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

        [SerializeField]
        string recipient;
        [SerializeField]
        string sender;
        [SerializeField]
        string messageBody;
        [SerializeField]
        string urgency;
        [SerializeField]
        int urgencyIndex;
        [SerializeField]
        bool isDelivered;

        [SerializeField]
        int messageID;

        // For debugging purposes
        string filepath;

        public Message(int i, string r, string s, string m, int u, bool d)
        {
            messageID = i;
            recipient = r;
            sender = s;
            messageBody = m;
            urgencyIndex = u;
            urgency = URGENCY_STATUS[u];
            isDelivered = d;
        }

        // Deep Copy Constructor
        public Message(Message m)
        {
            messageID = m.ID;
            recipient = m.Recipient;
            sender = m.Sender;
            messageBody = m.MessageBody;
            urgency = m.Urgency;
            urgencyIndex = m.UrgencyIndex;
            isDelivered = m.HasBeenDelivered;
        }

        public int ID
        {
            get
            {
                return messageID;
            }
        }

        public string Recipient
        {
            get
            {
                return recipient;
            }
        }

        public string Sender
        {
            get
            {
                return sender;
            }
        }

        public string MessageBody
        {
            get
            {
                return messageBody;
            }
        }

        public string Urgency
        {
            get
            {
                return URGENCY_STATUS[urgencyIndex];
            }
        }

        public int UrgencyIndex
        {
            get
            {
                return urgencyIndex;
            }
        }

        public bool HasBeenDelivered
        {
            get
            {
                return isDelivered;
            }
            set
            {
                isDelivered = value;
            }
        }

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
            
            return new Message(id, recipient, sender, message, urgencyIndex, false);
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
        public static Message ParseMessage(int id, string r, string s, string m, string u, bool d)
        {
            Message thisMessage = ParseMessage(id, r, s, m, u);
            thisMessage.HasBeenDelivered = d;

            return thisMessage;
        }
    }
}
