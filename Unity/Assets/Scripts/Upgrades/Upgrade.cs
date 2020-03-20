using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Upgrade
    {
        public Upgrade(string t, int c)
        {
            Title = t;
            Cost = c;
            IsUnlocked = false;
        }

        public void Purchase()
        {
            IsUnlocked = true;
            Quantity++;
        }
        
        public string Title { get; private set; }
        public int Cost { get; private set; }
        public bool IsUnlocked { get; private set; }
        public bool IsRepeatable { get; private set; }
        public int Quantity { get; private set; }
    }
}
