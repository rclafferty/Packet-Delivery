/* File: Upgrade.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */
 
namespace Assets.Scripts
{
    public class Upgrade
    {
        public Upgrade(string t, int c, bool r)
        {
            Title = t;
            Cost = c;
            IsUnlocked = false;
            IsRepeatable = r;
        }

        public void Purchase()
        {
            IsUnlocked = true;
            Quantity++;
        }

        public void Reset()
        {
            IsUnlocked = false;
            Quantity = 0;
        }
        
        public string Title { get; private set; }
        public int Cost { get; private set; }
        public bool IsUnlocked { get; private set; }
        public bool IsRepeatable { get; private set; }
        public int Quantity { get; private set; }
    }
}
