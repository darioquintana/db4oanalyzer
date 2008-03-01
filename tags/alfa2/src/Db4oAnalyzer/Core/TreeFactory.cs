using System.Windows.Forms;

namespace Db4oAnalyzer
{
    public class TreeFactory
    {
        private static TreeFactory _TreeFactory;

        private static TreeView _Tree;

        private TreeFactory()
        {
        }

        public static TreeFactory Instance
        {
            get
            {
                if (_TreeFactory == null)

                    _TreeFactory = new TreeFactory();

                return _TreeFactory;
            }
        }

        public void SetTreeView(TreeView tree)
        {
            _Tree = tree;
        }

        public TreeView Tree
        {
            get
            {
                return _Tree;
            }
        }
    }
}
