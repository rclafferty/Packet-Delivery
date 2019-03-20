using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Lookup_Agencies
{
    public class Person
    {
        string name;
        string location;
        int locationIndex;

        public Person(string n, string l, int li)
        {
            name = n;
            location = l;
            locationIndex = li;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Location
        {
            get
            {
                return location;
            }
        }

        public int LocationIndex
        {
            get
            {
                return locationIndex;
            }
        }
    }
}
