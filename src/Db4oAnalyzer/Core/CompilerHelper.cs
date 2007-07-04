using System;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Db4oAnalyzer
{
	public class CompilerHelper
	{
		public Assembly BuildAssembly(string code, string[] references, out string errors)
		{
			CodeDomProvider compiler = CodeDomProvider.CreateProvider("CSharp");

			//parametros
			CompilerParameters compilerparams = new CompilerParameters();
			compilerparams.GenerateExecutable = false;
			compilerparams.GenerateInMemory = true;
			//compilerparams.CompilerOptions = "/target:library"; // you can add /optimize
			
			References.AddRange(references);
			compilerparams.ReferencedAssemblies.AddRange(References.ToArrayWithPostfix());
			
			CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);

			if (results.Errors.HasErrors)
			{
				StringBuilder errorsSb = new StringBuilder("Compiler Errors :\r\n");
				foreach (CompilerError error in results.Errors)
				{
					errorsSb.AppendLine(string.Format("Line {0},{1}\t: {2}\n", error.Line, error.Column, error.ErrorText));
				}
				errors = errorsSb.ToString();
				return null;
			}
			else
			{
				errors = string.Empty;
				return results.CompiledAssembly;
			}
		}

        
		public string Run(string code, string[] references, out string errors)
		{
			Assembly assembly = BuildAssembly(code, references, out errors);
			IOHelper.Save(code,"log.txt");
			if (assembly == null) return string.Empty;

			CallEntry(assembly, "Main");

			return code;
		}

		private void CallEntry(Assembly assembly, string entryPoint)
		{
			try
			{
				//Use reflection to call the static Main function
				Module[] mods = assembly.GetModules(false);
				Type[] types = mods[0].GetTypes();

				foreach (Type type in types)
				{
					MethodInfo mi = type.GetMethod(entryPoint,
					                               BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
					if (mi != null)
					{
						if (mi.GetParameters().Length == 1)
						{
							if (mi.GetParameters()[0].ParameterType.IsArray)
							{
								string[] par = new string[1]; // if Main has string [] arguments
								mi.Invoke(null, par);
							}
						}
						else
						{
							mi.Invoke(null, null);
						}
						return;
					}

				}

				//LogErrMsgs("Engine could not find the public static " + entryPoint );
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				//Console.WriteLine(ex.InnerException.Message);
			}

		}

	}
}
