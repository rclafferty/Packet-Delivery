using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Lookup_Agencies
{
    public class Person
    {
        [Obsolete("Please include the address, not the location / index")]
        public Person(string n, string l, int li)
        {
            Name = n;
            Location = l;
            LocationIndex = li;
        }

        public Person(string n, string a, string u)
        {
            Name = n;
            Address = a;
            URL = u;
        }

        public string Name { get; private set; }

        public string Location { get; private set;}

        public int LocationIndex { get; private set;}

        public string Address { get; private set; }

        public string URL { get; private set; }
    }
}
