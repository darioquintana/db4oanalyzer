using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Db4oAnalyzer.Core.Cfg
{
	public class ReferenceCollection : ConfigurationElementCollection
	{
		public ReferenceCollection()
		{
		}
		
		public override
			ConfigurationElementCollectionType CollectionType
		{
			get
			{
				return
					ConfigurationElementCollectionType.AddRemoveClearMap;
			}
		}
		protected override
			ConfigurationElement CreateNewElement()
		{
			return new ReferenceElement();
		}
		
		protected override
			ConfigurationElement CreateNewElement(
				string elementName)
		{
			return new ReferenceElement(elementName);
		}
		
		protected override Object
			GetElementKey(ConfigurationElement element)
		{
			return string.Format("{0}", ((ReferenceElement)element).AssemblyName);
		}
		
		public new string AddElementName
		{
			get
			{ return base.AddElementName; }

			set
			{ base.AddElementName = value; }

		}
		
		public new string ClearElementName
		{
			get { return base.ClearElementName; }

			set { base.AddElementName = value; }

		}
		
		public new string RemoveElementName
		{
			get	{ return base.RemoveElementName; }
		}
		
		public new int Count
		{
			get { return base.Count; }
		}
		
		public ReferenceElement this[int index]
		{
			get
			{
				return (ReferenceElement)BaseGet(index);
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index, value);
			}
		}
		
		new public ReferenceElement this[string Name]
		{
			get
			{
				return (ReferenceElement)BaseGet(Name);
			}
		}
		
		public int IndexOf(ReferenceElement psh)
		{
			return BaseIndexOf(psh);
		}
		
		public void Add(ReferenceElement psh)
		{
			BaseAdd(psh);
		}
		
		protected override void
			BaseAdd(ConfigurationElement element)
		{
			BaseAdd(element, true);
		}
		
		public void Remove(ReferenceElement psh)
		{
			if (BaseIndexOf(psh) >= 0)
				BaseRemove(psh.AssemblyName);
		}
		
		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}
		
		public void Remove(string name)
		{
			BaseRemove(name);
		}
		
		public void Clear()
		{
			BaseClear();
		}
	}
	
}
