namespace Db4oAnalyzer
{
    public struct Azzembly
    {
        public string Name;
        public PostfixAzzembly Postfix;
    }

    public struct PostfixAzzembly
    {
        public static string Exe = ".exe";
        public static string Dll = ".dll";
        public static string None = string.Empty;
    }
}
