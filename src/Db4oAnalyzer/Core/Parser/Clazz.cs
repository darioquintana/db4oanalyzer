using System.Collections.Generic;

namespace Db4oAnalyzer.Core.Parser
{
    public class Clazz
    {
        private string _Name;
        private string _FullName;
        private bool _IsValueType;
        private List<Field> _items = new List<Field>();
        private string _Namespace;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        
        public bool IsValueType
        {
            get { return _IsValueType; }
            set { _IsValueType = value; }
        }

        public List<Field> FieldCollection
        {
            get { return _items; }
            set { _items = value; }
        }

        public string Namespace
        {
            get { return _Namespace; }
            set { _Namespace = value; }
        }
    }
}