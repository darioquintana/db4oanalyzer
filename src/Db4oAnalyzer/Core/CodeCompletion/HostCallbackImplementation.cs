﻿using System;
using System.Windows.Forms;
using ICSharpCode.SharpDevelop.Dom;

namespace Db4oAnalyzer
{
	/// <summary>
	/// ICSharpCode.SharpDevelop.Dom was created by extracting code from ICSharpCode.SharpDevelop.dll.
	/// There are a few static method calls that refer to GUI code or the code for keeping the parse
	/// information. These calls have to be implemented by the application hosting
	/// ICSharpCode.SharpDevelop.Dom by settings static fields with a delegate to their method
	/// implementation.
	/// </summary>
	static class HostCallbackImplementation
	{
		public static void Register(MainForm mainForm)
		{
			// Must be implemented. Gets the parse information for the specified file.
			HostCallback.GetParseInformation = delegate(string fileName) {
				if (fileName != MainForm.DummyFileName)
					throw new Exception("Unknown file");
				if (mainForm.lastCompilationUnit == null)
					return null;
				ParseInformation pi = new ParseInformation();
				pi.ValidCompilationUnit = mainForm.lastCompilationUnit;
				return pi;
			};
			
			// Must be implemented. Gets the project content of the active project.
			HostCallback.GetCurrentProjectContent = delegate {
				return mainForm.myProjectContent;
			};
			
			// The default implementation just logs to Log4Net. We want to display a MessageBox.
			// Note that we use += here - in this case, we want to keep the default Log4Net implementation.
			HostCallback.ShowError += delegate(string message, Exception ex) {
				MessageBox.Show(message + Environment.NewLine + ex.ToString());
			};
			HostCallback.ShowMessage += delegate(string message) {
				MessageBox.Show(message);
			};
			HostCallback.ShowAssemblyLoadError += delegate(string fileName, string include, string message) {
				MessageBox.Show("Error loading code-completion information for "
				                + include + " from " + fileName
				                + ":\r\n" + message + "\r\n");
			};
		}
	}
}
