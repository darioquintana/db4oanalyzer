using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.Collections;
using Db4objects.Db4o;

namespace Db4oAnalyzer
{
    public class ObjectTree
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oSet"></param>
        public static void Draw(IObjectSet oSet)
        {
            IEnumerator ie = oSet.GetEnumerator();

            List<object> list = new List<object>();

            while (ie.MoveNext())
            {
                list.Add(ie.Current);
            }

            Draw(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        public static void Draw<T>(IEnumerable<T> enumerable)
        {
            //clean the current tree
            TreeFactory.Instance.Tree.Nodes.Clear();

            int i = 0;

            IEnumerator<T> ie = enumerable.GetEnumerator();
            while (ie.MoveNext())
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Object: {0}", i + 1);
                Console.WriteLine("Value: {0}", ie.Current);

                TreeNode node;

                node = GetNodes(ie.Current);

                node.Text = string.Format("{0} -> {1} ", i + 1, node.Text);

                TreeFactory.Instance.Tree.Nodes.Add(node);

                i++;
            }
        }
        
        private static TreeNode GetNodes<T>(T item)
        {
            Console.WriteLine("Type Item: {0}", item.GetType().Name);

            FieldInfo[] fi = item.GetType().GetFields(

                (BindingFlags.Static | BindingFlags.NonPublic) |
                (BindingFlags.NonPublic) |
                (BindingFlags.Public | BindingFlags.Static) |
                (BindingFlags.Public) |
                (BindingFlags.NonPublic | BindingFlags.Instance)
                );

            return MakeNodeByFields(item, fi);

        }


        private static TreeNode MakeNodeByFields<T>(T obj, FieldInfo[] items)
        {
            TreeNode nodes = new TreeNode();

            nodes.Text = obj.ToString();

            foreach (FieldInfo item in items)
            {
                nodes.Nodes.Add(MakeNode(obj, item));
            }

            return nodes;
        }

        private static TreeNode MakeNode<T>(T obj, FieldInfo item)
        {
            try
            {
                TreeNode node = new TreeNode();

                node.Text = string.Concat(node.Text, item.Name);
                node.ImageIndex = 3; //Correspond to a Field image.
                node.SelectedImageIndex = 3;

                object result = obj.GetType().InvokeMember(item.Name.ToString(),
                    (BindingFlags.GetField | BindingFlags.NonPublic) |
                    (BindingFlags.GetField | BindingFlags.Static) |
                    (BindingFlags.GetField | BindingFlags.Public) |
                    (BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance)
                    , null, obj, new object[] { });

                Console.WriteLine("fieldname: {0}", item.Name);
                Console.WriteLine("fieldvalue: {0}", result.ToString());

                TreeNode valueNode = new TreeNode();
                valueNode.Text = result.ToString();
                valueNode.ImageIndex = 8;
                valueNode.SelectedImageIndex = 8;

                node.Nodes.Add(valueNode);

                return node;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
