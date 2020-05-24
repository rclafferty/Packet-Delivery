/* File: Person.cs
 * Author: Casey Lafferty
 * Project: Packet Delivery
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Lookup_Agencies
{
    [Serializable]
    public class Person
    {
        public Person(string n, string u, string ne, char ni, int h)
        {
            Name = n;
            URL = u;
            Neighborhood = ne;
            NeighborhoodID = ni;
            HouseNumber = h;
        }

        public string Name { get; private set; }
        public string URL { get; private set; }
        public string Neighborhood { get; private set; } // Full expanded neighborhood name
        public char NeighborhoodID { get; private set; } // ID to lookup the neighborhood name if needed
        public int HouseNumber { get; private set; }
    }
}
