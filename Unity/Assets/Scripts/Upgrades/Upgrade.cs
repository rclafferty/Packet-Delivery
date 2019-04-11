using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    class Upgrade
    {
        private string name;
        private int cost;
        private int quantity;
        private int quantity_max;

        public Upgrade(string n, int c, int q)
        {
            name = n;
            cost = c;
            quantity = q;
            quantity_max = q;
        }

        /// <summary>
        /// The name of the upgrade
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// The cost to improve this upgrade
        /// </summary>
        public int Cost
        {
            get
            {
                return cost;
            }
        }

        /// <summary>
        /// The amount of this type of upgrade currently available
        /// </summary>
        public int Quantity
        {
            get
            {
                return quantity;
            }
        }

        public void Purchase(int q)
        {
            quantity += q;
        }

        public void Remove(int q)
        {
            quantity -= q;
        }
    }
}
