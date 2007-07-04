using System.Windows.Forms;
using System.Text;
using System.IO;


namespace Db4oAnalyzer
{
    class ConsoleToTexbox : TextWriter
    {
        private TextBox _TextBox;

        public ConsoleToTexbox(TextBox textBox)
            : base()
        {
            _TextBox = textBox;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(string value)
        {
            _TextBox.AppendText(value.Replace("\n", base.NewLine));
        }

        public override void WriteLine(string value)
        {
            _TextBox.AppendText(value);
            _TextBox.AppendText(NewLine);
        }

        public override void WriteLine(int value)
        {
            WriteLine(value.ToString());
        }

        public override void WriteLine(decimal value)
        {
            WriteLine(value.ToString());
        }
        public override void WriteLine(bool value)
        {
            WriteLine(value.ToString());
        }
        public override void WriteLine(long value)
        {
            WriteLine(value.ToString());
        }

       



    }
}
