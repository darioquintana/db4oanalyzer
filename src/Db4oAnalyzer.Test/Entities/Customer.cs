using System;
using System.Collections.Generic;

namespace Db4oAnalyzer.Test.Entities
{
    public class Customer
    {
        private Guid id;
        private string name;
        private string secondName;
        private List<Address> addresses;

        public Customer()
        {
            addresses = new List<Address>();
        }

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string SecondName
        {
            get { return secondName; }
            set { secondName = value; }
        }

        public List<Address> Addresses
        {
            get { return addresses; }
            set { addresses = value; }
        }
    }
}