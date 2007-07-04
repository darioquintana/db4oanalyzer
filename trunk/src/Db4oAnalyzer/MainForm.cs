using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using NRefactory = ICSharpCode.NRefactory;
using Dom = ICSharpCode.SharpDevelop.Dom;

namespace Db4oAnalyzer
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        //fields
        private ConsoleToTexbox ctt;
        public const string DummyFileName = "edited.cs";
        internal Dom.ProjectContentRegistry pcRegistry;
        internal Dom.DefaultProjectContent myProjectContent;
        internal Dom.ICompilationUnit lastCompilationUnit;


        private Thread parserThread;
        private ManualResetEvent m_EventStopThread;
        private ManualResetEvent m_EventThreadStopped;

        public DelegateAddString m_DelegateAddString;
        public DelegateThreadFinished m_DelegateThreadFinished;
        private bool thereIsNewReferencesToAdd;

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public MainForm()
        {
            InitializeComponent();

            //AtLoad();

            ConfigureEditorTextControl();

            tsbGo.Image = imglist.Images[0];
        }

        private void MainFormLoad(object sender, EventArgs e)
        {
            AtLoad();
        }

        private void AtLoad()
        {
            //Redirect the output Console to the ouput Textbox

            TreeFactory.Instance.SetTreeView(this.TreeViewObj);

            SetPathOfDatabase();
            LoadFromPreviusSession();
            ctt = new ConsoleToTexbox(txtRes);
            Console.SetOut(ctt);

            LoadListboxReferences();

            m_EventStopThread = new ManualResetEvent(false);
            m_EventThreadStopped = new ManualResetEvent(false);
        }


        private void LoadFromPreviusSession()
        {
            try
            {
                txtCode.Text = IOHelper.Get("previous_session_code");
                //txtReferences.Text = IOHelper.Get("previous_session_references");
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error to attemp load the session");
                Console.WriteLine(ex.Message);
            }
        }

        private void SaveCurrentSession()
        {
            try
            {
                IOHelper.Save(txtCode.Text, "previous_session_code");
                //IOHelper.Save(txtReferences.Text,"previous_session_references");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error to attemp save the session");
                Console.WriteLine(ex.Message);
            }
        }

        private void btnAnalize_Click(object sender, EventArgs e)
        {
            Go();
        }

        private void Limpiar()
        {
            txtRes.Clear();
            txtCodeGenerated.Clear();
            txtErrors.Clear();
        }

        public string[] GetReferences()
        {
            //return txtReferences.Text.Split(',');
            return null;
        }

        private void LoadListboxReferences()
        {
            lstReferences.Items.AddRange(References.ToArrayWithPostfix());
        }


        private void AboutDb4oAnalyzerToolStripMenuItemClick(object sender, EventArgs e)
        {
            About frm = new About();
            frm.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCurrentSession();
        }

        private void txtPath_TextChanged(object sender, EventArgs e)
        {
            SetPathOfDatabase();
        }

        private void SetPathOfDatabase()
        {
            Db4oHelper.Instance.SavePath(txtPath.Text);
        }

        private void Go()
        {
            Limpiar();
            CompilerHelper compiler = new CompilerHelper();
            string errors = string.Empty;
            txtCodeGenerated.Text = compiler.Run(txtCode.Text, GetReferences(), out errors);
            txtErrors.Text = errors;
        }

        private void GoToolStripMenuItemClick(object sender, EventArgs e)
        {
            Go();
        }

        private void TsbGoClick(object sender, EventArgs e)
        {
            Go();
        }


        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ConfigureEditorTextControl()
        {
            txtCode.SetHighlighting("C#");
            txtCode.ShowEOLMarkers = false;
            CodeCompletionKeyHandler.Attach(this, txtCode);
            HostCallbackImplementation.Register(this);

            pcRegistry = new Dom.ProjectContentRegistry(); // Default .NET 2.0 registry

            // Persistence caches referenced project contents for faster loading.
            // It also activates loading XML documentation files and caching them
            // for faster loading and lower memory usage.
            pcRegistry.ActivatePersistence(Path.Combine(Path.GetTempPath(), "CSharpCodeCompletion"));

            myProjectContent = new Dom.DefaultProjectContent();
            myProjectContent.Language = Dom.LanguageProperties.CSharp;
        }

        protected override void OnLoad(EventArgs e)
        {
            if (e == null)
            {
                MessageBox.Show("es nulo");
            }
            base.OnLoad(e);

            RunThread();
        }

        private void RunThread()
        {
            parserThread = new Thread(ParserThread);
            parserThread.IsBackground = true;
            parserThread.Start();
        }

        //se lanza una vez
        private void ParserThread()
        {
            BeginInvoke(new MethodInvoker(delegate { parserThreadLabel.Text = "Loading mscorlib..."; }));
            myProjectContent.AddReferencedContent(pcRegistry.Mscorlib);

            // do one initial parser step to enable code-completion while other
            // references are loading
            ParseStep();

            //string[] refsInWindows = IOHelper.Get("previous_session_references").Split(',');

            string[] references = References.ToArrayWithoutPostfix();

            List<string> refs = new List<string>();

            //refs.AddRange(refsInWindows);
            refs.AddRange(references);

            foreach (string assemblyName in refs)
            {
                {
                    // block for anonymous method
                    string assemblyNameCopy = assemblyName;
                    BeginInvoke(
                        new MethodInvoker(delegate { parserThreadLabel.Text = "Loading " + assemblyNameCopy + "..."; }));
                }
                myProjectContent.AddReferencedContent(
                    pcRegistry.GetProjectContentForReference(assemblyName, assemblyName));
            }
            BeginInvoke(new MethodInvoker(delegate { parserThreadLabel.Text = "Ready"; }));

            // Parse the current file every 2 seconds
                    
            while (!IsDisposed && !ThereIsNewReferencesToAdd)
            {
                ParseStep();

                Thread.Sleep(2000);
            }
        }

        private bool ThereIsNewReferencesToAdd
        {
            get { return thereIsNewReferencesToAdd; }
            set { thereIsNewReferencesToAdd = value; }
        }

        // cada 2 segundos se lanza este metodo.
        private void ParseStep()
        {
            string code = null;
            Invoke(new MethodInvoker(delegate { code = txtCode.Text; }));
            TextReader textReader = new StringReader(code);
            Dom.ICompilationUnit newCompilationUnit;
            using (NRefactory.IParser p = NRefactory.ParserFactory.CreateParser(NRefactory.SupportedLanguage.CSharp, textReader))
            {
                p.Parse();
                newCompilationUnit = ConvertCompilationUnit(p.CompilationUnit);
            }
            // Remove information from lastCompilationUnit and add information from newCompilationUnit.
            myProjectContent.UpdateCompilationUnit(lastCompilationUnit, newCompilationUnit, DummyFileName);
            lastCompilationUnit = newCompilationUnit;
        }

        private Dom.ICompilationUnit ConvertCompilationUnit(NRefactory.Ast.CompilationUnit cu)
        {
            Dom.NRefactoryResolver.NRefactoryASTConvertVisitor converter;
            converter = new Dom.NRefactoryResolver.NRefactoryASTConvertVisitor(myProjectContent);
            cu.AcceptVisitor(converter, null);
            return converter.Cu;
        }

        private void StopThread()
        {
            if (parserThread != null && parserThread.IsAlive) // thread is active
            {
                // set event "Stop"
                m_EventStopThread.Set();

                // wait when thread  will stop or finish
                while (parserThread.IsAlive)
                {
                    // We cannot use here infinite wait because our thread
                    // makes syncronous calls to main form, this will cause deadlock.
                    // Instead of this we wait for event some appropriate time
                    // (and by the way give time to worker thread) and
                    // process events. These events may contain Invoke calls.
                    if (WaitHandle.WaitAll(
                        (new ManualResetEvent[] {m_EventThreadStopped}),
                        100,
                        true))
                    {
                        break;
                    }

                    Application.DoEvents();
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReferences frm = new frmReferences();
            frm.ShowDialog();
        }
    }

    // delegates used to call MainForm functions from worker thread
    public delegate void DelegateAddString(String s);

    public delegate void DelegateThreadFinished();
}