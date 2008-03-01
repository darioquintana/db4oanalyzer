using System.Collections.Generic;
using Db4objects.Db4o;

namespace Db4oAnalyzer
{
	/// <summary>
	/// Description of ProgramBase.
	/// </summary>
	public abstract class ProgramBase
	{
		#region fields
		public static IObjectContainer _db;
		
		
		#endregion
		
		#region methods

        #region property readonly db
        public static IObjectContainer db
		{
			get
			{
                if (_db == null || _db.Ext().IsClosed())
                {
                    _db = Db4oHelper.Instance.GetConnection();
                }

			    return _db;
			}
        }
        #endregion

        #region Functions draw(...);
        
        public static void draw<T>(IList<T> list)
		{
            IEnumerable<T> enumerable = list;
            ObjectTree.Draw(enumerable);
		}

        public static void draw(IObjectSet objectSet)
        {
            ObjectTree.Draw(objectSet);
        }

        public static void draw<T>(IEnumerable<T> enumerable)
        {
            ObjectTree.Draw(enumerable);
        }

	    #endregion

        #endregion


		
	}
}
