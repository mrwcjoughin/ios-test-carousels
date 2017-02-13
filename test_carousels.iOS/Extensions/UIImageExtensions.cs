using System;
using System.Net.Http;
using System.Threading.Tasks;
using Foundation;
using test_carousels.Core;
using UIKit;

namespace test_carousels.iOS.Extension
{
	public static class UIImageExtensions
	{
		public static UIImage FromUrl (string uri)
		{
			using (var url = new NSUrl (uri))
			using (var data = NSData.FromUrl (url))
				return UIImage.LoadFromData (data);
		}

		public static async Task<UIImage> LoadImage (string imageUrl)
		{
			var contents = await HttpHelper.LoadData(imageUrl);;

			// load from bytes
			return UIImage.LoadFromData (NSData.FromArray (contents));
		}
	}
}