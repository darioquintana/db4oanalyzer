using System.Configuration;
using Db4oAnalyzer.Core.Cfg;

namespace Db4oAnalyzer.Core.Cfg
{
	public class Db4oAnalyzerSection : ConfigurationSection
	{

		public Db4oAnalyzerSection()
		{
		}

		[ConfigurationProperty("references", IsDefaultCollection = false)]
		public ReferenceCollection References
		{
			get
			{
				ReferenceCollection references = (ReferenceCollection)base["references"];
				return references;
			}
		}

		protected override void DeserializeSection(
			System.Xml.XmlReader reader)
		{
			base.DeserializeSection(reader);
		}

		protected override string SerializeSection(
			ConfigurationElement parentElement,
			string name, ConfigurationSaveMode saveMode)
		{
			string s =
				base.SerializeSection(parentElement,name, saveMode);
			return s;
		}
	}
}
