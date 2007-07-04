using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Db4oAnalyzer
{
    public partial class frmReferences : Form
    {
        public frmReferences()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
        }

        private void frmReferences_Load(object sender, EventArgs e)
        {
            List<AzzemblyInGAC> list = GetGACCollection(@"\assembly\GAC");
            list.AddRange(GetGACCollection(@"\assembly\GAC_MSIL"));

            foreach (AzzemblyInGAC ass in list)
            {
                ListViewItem li = new ListViewItem(ass.Name);
                li.Tag = ass.Tag;
                li.SubItems.Add(ass.Version);
                // li.SubItems.Add(ass.Culture);
                li.SubItems.Add(ass.Key);
                li.SubItems.Add(ass.Path);
                lvGAC.Items.Add(li);
            }
        }

        public List<AzzemblyInGAC> GetGACCollection(string postfix)
        {
            List<AzzemblyInGAC> list = new List<AzzemblyInGAC>();

            string gacPath = Environment.GetEnvironmentVariable("SystemRoot") + postfix;

            DirectoryInfo gacDI = new DirectoryInfo(gacPath);

            DirectoryInfo[] assemblies = gacDI.GetDirectories();


            Graphics g = CreateGraphics();
            int maxNameWidth = 0;
            int maxVersionWidth = 0;
            int maxCultureWidth = 0;
            int maxKeyWidth = 0;

            foreach (DirectoryInfo di in assemblies)
            {
                int width;
                // The one subdirectory is the string that contains
                // version_culture_publickey (separated by underscores)
                DirectoryInfo[] subDirDis = di.GetDirectories();
                String subDirName = subDirDis[0].Name;

                width = (int) g.MeasureString(subDirName, Font).Width;
                if (width > maxNameWidth)
                    maxNameWidth = width;

                int culturePos = subDirName.IndexOf('_');
                int keyPos = subDirName.IndexOf('_', culturePos + 1);
                String version = subDirName.Substring(0, culturePos);
                width = (int) g.MeasureString(version, Font).Width;
                if (width > maxVersionWidth)
                    maxVersionWidth = width;

                String culture = subDirName.Substring(culturePos + 1,
                                                      keyPos - culturePos - 1);
                width = (int) g.MeasureString(culture, Font).Width;
                if (width > maxCultureWidth)
                    maxCultureWidth = width;

                String key = subDirName.Substring(keyPos + 1);
                width = (int) g.MeasureString(key, Font).Width;
                if (width > maxKeyWidth)
                    maxKeyWidth = width;

                FileInfo[] dlls = subDirDis[0].GetFiles("*.dll");

                object Tag = null;

                if (dlls.Length == 1)
                    Tag = dlls[0];

                list.Add(new AzzemblyInGAC(di.Name, version, di.FullName, di.FullName, culture, key, Tag));
            }

            g.Dispose();

            return list;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


    public class AzzemblyInGAC
    {
        private string name;
        private string version;
        private string path;
        private string fullName;
        private string culture;
        private string key;
        private object tag;


        public AzzemblyInGAC(string name, string version, string path, string fullName, string culture, string key,
                             object tag)
        {
            this.name = name;
            this.version = version;
            this.path = path;
            this.fullName = fullName;
            this.culture = culture;
            this.key = key;
            this.tag = tag;
        }

        public AzzemblyInGAC()
        {
        }

        public override string ToString()
        {
            return
                string.Concat(this.Name, " | ", this.FullName, " | ", this.Culture, " | ", this.Version, " | ", this.Key);
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Culture
        {
            get { return culture; }
            set { culture = value; }
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }

        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }
    }
}