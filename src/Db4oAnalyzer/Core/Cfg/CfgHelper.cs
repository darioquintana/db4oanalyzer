using System.Configuration;

namespace Db4oAnalyzer.Core.Cfg
{
	public sealed class CfgHelper
	{
		static Configuration cfg;
		
		#region Singleton
		private CfgHelper()
		{
			cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		}
		
		private static CfgHelper _CfgHelper = null;
		
		public static CfgHelper Instance
		{
			get
			{
				if(_CfgHelper == null)
				{
					_CfgHelper = new CfgHelper();
				}
				
				return _CfgHelper;
			}
		}
		#endregion
		
		public Db4oAnalyzerSection GetSection
		{
			get
			{
				Db4oAnalyzerSection cs = cfg.Sections["db4oanalyzer"] as Db4oAnalyzerSection;
				
				return cs;
			}
		}
	}
}
