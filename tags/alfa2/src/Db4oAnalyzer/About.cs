using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace Db4oAnalyzer
{
	/// <summary>
	/// Description of About.
	/// </summary>
	public partial class About : Form
	{
		public About()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void AboutLoad(object sender, EventArgs e)
		{
			linkBlog.Links.Add(0,linkBlog.Text.Length ,"http://blog.darioquintana.com.ar");
			linkWeb.Links.Add(0,linkWeb.Text.Length,"http://darioquintana.com.ar/projects/db4oanalayzer/");
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
			
		
		}
		
		void LinkWebLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
			Process.Start(sInfo);
		}
		
		void LinkBlogLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			ProcessStartInfo sInfo = new ProcessStartInfo(e.Link.LinkData.ToString());
			Process.Start(sInfo);
		}
	}
}
