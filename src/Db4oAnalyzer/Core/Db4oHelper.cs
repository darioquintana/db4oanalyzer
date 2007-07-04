using System;
using Db4objects.Db4o;

namespace Db4oAnalyzer
{
    public sealed class Db4oHelper 
    {
        private static Db4oHelper _Db4oHelper;

        private Db4oHelper()
        { }

        public static  Db4oHelper Instance
        {
            get
            {
                if (_Db4oHelper == null)
                {
                    _Db4oHelper = new Db4oHelper();
                }
                return _Db4oHelper;
            }
        }

        public string _Path = string.Empty;

        public IObjectContainer GetConnection()
        {
            if (_Path != string.Empty)
            {
                //Console.WriteLine(string.Concat("Path of database:", _Path));
                return Db4oFactory.OpenFile(_Path);
            }
            else
            {
                throw new Exception("the path it's empty");
            }
        }

        public void SavePath(string path)
        {
            _Path = path;
        }

        
       
    }
}
