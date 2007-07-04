using System.Configuration;


namespace Db4oAnalyzer.Core.Cfg
{
	public class ReferenceElement : ConfigurationElement
	{
		public ReferenceElement()
		{
		}
		
		internal ReferenceElement(string assemblyName)
            : this()
        {
            this["name"] = assemblyName;
        }      
       
		[ConfigurationProperty("name", IsKey=true, IsRequired=true)]
		public string AssemblyName 
		{
			get { return (string)this["name"]; } 
			set { this["name"] = value; } 
		}
		
		[ConfigurationProperty("framework")]
		public bool Framework 
		{
			get { return (bool)this["framework"]; } 
			set { this["framework"] = value; } 
		}
		

		
	}
}
