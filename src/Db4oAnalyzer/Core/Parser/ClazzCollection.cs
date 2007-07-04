using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Db4oAnalyzer.Core.Parser
{
    public class ClazzCollection : IList<Clazz>
    {
        private List<Clazz> _items;

        public ClazzCollection()
        {
            _items = new List<Clazz>();
        }

        #region IList<Clazz> Members

        public int IndexOf(Clazz item)
        {
            return _items.IndexOf(item);
        }

        public void Insert(int index, Clazz item)
        {
            _items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _items.RemoveAt(index);
        }

        public Clazz this[int index]
        {
            get { return _items[index]; }
            set { _items[index] = value; }
        }

        #endregion

        #region ICollection<Clazz> Members

        public void Add(Clazz item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(Clazz item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Clazz[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        public bool Remove(Clazz item)
        {
            return _items.Remove(item);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<Clazz> Members

        public IEnumerator<Clazz> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        #endregion
    }

}
