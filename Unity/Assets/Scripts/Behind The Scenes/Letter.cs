using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        public int ID { get; private set; }
        public Person Sender { get; private set; }
        public Person Recipient { get; private set; }
        public string Body { get; private set; }
        public bool IsDelivered { get; private set; }
    }
}
