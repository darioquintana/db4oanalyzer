using System;
using System.IO;

namespace Db4oAnalyzer
{

	public class IOHelper
	{
		public static void Save(string text, string fileName)
		{
			try
			{
				StreamWriter sw = new StreamWriter(fileName);
				sw.Write(text);
				sw.Flush();
				sw.Close();
			}
			catch (Exception ex)
			{
			    throw ex;
                //Console.WriteLine(string.Format("Error when attempting save in the file {0}", fileName));
				//Console.WriteLine(ex.Message);

			}
		}
		
		public static string Get(string fileName)
		{
			try
			{
				StreamReader sr = new StreamReader(fileName);
				string text = sr.ReadToEnd();
				sr.Close();
				return text;
			}

			catch (Exception ex)
			{
				//Console.WriteLine(string.Format("Error when attempting read the file {0}",fileName));
				//Console.WriteLine(ex.Message);
				return string.Empty;
			}
			
		}
		
	}
}
