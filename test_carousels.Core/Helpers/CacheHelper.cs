using System;
using System.Collections.Generic;
using System.Linq;

namespace test_carousels.Core
{
	public class CacheHelper
	{
		#region Fields

		public static CacheHelper SharedInstance = new CacheHelper();

		private List<CacheHelperItem> cacheHelperItemList = new List<CacheHelperItem>();

		#endregion Fields

		#region Methods

		public void AddToCache(string key, object value)
		{
			cacheHelperItemList.Add(new CacheHelperItem() { Key = key, Value = value });
		}

		public object GetFromCache(string key)
		{
			var result = cacheHelperItemList.Where(t => t.Key == key).FirstOrDefault();

			if (result != null)
			{
				return result.Value;
			}

			return null;
		}

		#endregion Methods
	}

	public class CacheHelperItem
	{
		public string Key
		{
			get;
			set;
		}

		public object Value
		{
			get;
			set;
		}
	}
}
