using System.Collections.Generic;

using config = Db4oAnalyzer.Core.Cfg;

namespace Db4oAnalyzer
{
	public class References
	{
		static IDictionary<string,string> _DictRefs = new Dictionary<string,string>();

		public void Add(string reference,string postfix)
		{
			_DictRefs.Add(reference,postfix);
		}
		
		public static string[] ToArrayWithPostfix()
		{
			if(_DictRefs.Count == 0) LoadReferences();
			
			string[] array = new string[_DictRefs.Count];
			int i = 0;

			foreach(KeyValuePair<string, string> item in _DictRefs)
			{
				array[i] = string.Concat(item.Key,item.Value);
				
				i++;
			}
			
			return array;
		}
		
		public static string[] ToArrayWithoutPostfix()
		{
			if(_DictRefs.Count == 0) LoadReferences();
			
			string[] array = new string[_DictRefs.Count];
			int i = 0;

			foreach(KeyValuePair<string, string> item in _DictRefs)
			{
				array[i] = item.Key;
				
				i++;
			}
			return array;
			
		}
		
		public static void AddRange(string[] array)
		{
			if (array == null) return;
			
			foreach(string item in array)
			{
				if(!_DictRefs.ContainsKey(item))
				{
					_DictRefs.Add(item,string.Empty);
				}
			}
		}

		private static void LoadReferences()
		{
            // Example of references without the passing by configuration
            //_DictRefs.Add("System.Data", PostfixAzzembly.Dll);
            //_DictRefs.Add("System.Drawing", PostfixAzzembly.Dll);
            //_DictRefs.Add("System.Xml", PostfixAzzembly.Dll);
            //_DictRefs.Add("System.Windows.Forms", PostfixAzzembly.Dll);
            //_DictRefs.Add("Db4objects.Db4o.dll", PostfixAzzembly.None);
			
			_DictRefs.Add("mscorlib", PostfixAzzembly.Dll);
			_DictRefs.Add("System", PostfixAzzembly.Dll);
			_DictRefs.Add("Db4oAnalyzer.exe", PostfixAzzembly.None);
			
			foreach(config.ReferenceElement item in config.CfgHelper.Instance.GetSection.References)
			{
				if(item.Framework)
				{
					if(!_DictRefs.ContainsKey(item.AssemblyName))
						_DictRefs.Add(item.AssemblyName,PostfixAzzembly.Dll);
				}
				else
				{
					if(!_DictRefs.ContainsKey(item.AssemblyName))
						_DictRefs.Add(item.AssemblyName,PostfixAzzembly.None);
				}
			}
			
		}
		

	}
}
