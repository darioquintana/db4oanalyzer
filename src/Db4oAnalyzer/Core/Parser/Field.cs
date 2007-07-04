namespace Db4oAnalyzer.Core.Parser
{
    public class Field
    {
        private string _Name;
        private bool _IsValueType;
        private string _TypeName;
        private string _TypeFullName;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public bool IsValueType
        {
            get { return _IsValueType; }
            set { _IsValueType = value; }
        }

        public string TypeName
        {
            get { return _TypeName; }
            set { _TypeName = value; }
        }

        public string TypeFullName
        {
            get { return _TypeFullName; }
            set { _TypeFullName = value; }
        }
    }
}