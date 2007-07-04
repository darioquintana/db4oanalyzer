namespace Db4oAnalyzer.Test.Entities
{
    public class Address
    {
        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public Address(string description)
        {
            Description = description;
        }

        public override string ToString()
        {
            return string.Format("Address: {0}", Description);
        }
    }

     
}
