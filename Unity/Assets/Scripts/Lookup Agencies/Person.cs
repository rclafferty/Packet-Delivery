using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Lookup_Agencies
{
    public class Person
    {
        public Person(string n, string l, int li)
        {
            Name = n;
            Location = l;
            LocationIndex = li;
        }

        public string Name { get; private set; }

        public string Location { get; private set;}

        public int LocationIndex { get; private set;}
    }
}
