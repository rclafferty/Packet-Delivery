/* File: Letter.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using System;
using System.Text;
using Assets.Scripts.Lookup_Agencies;

namespace Assets.Scripts.Behind_The_Scenes
{
    [Serializable]
    public class Letter
    {
        public Letter(int i, Person s, Person r, string b)
        {
            ID = i;
            Sender = s;
            Recipient = r;
            Body = b;
            IsDelivered = false;
        }

        public void MarkDelivered(bool isDelivered)
        {
            IsDelivered = isDelivered;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Letter Details:\n");
            sb.Append("ID: ");
            sb.Append(ID);
            sb.Append("\nSender: ");
            sb.Append(Sender != null);
            sb.Append("\nRecipient: ");
            sb.Append(Recipient != null);
            sb.Append("\nBody: ");
            sb.Append(Body);
            return sb.ToString();
        }
        
        public int ID { get; private set; }
        public Person Sender { get; private set; }
        public Person Recipient { get; private set; }
        public string Body { get; private set; }
        public bool IsDelivered { get; private set; }
    }
}
