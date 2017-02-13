using System;
using System.Threading.Tasks;

namespace test_carousels.Core.Models
{
	public class Movie
	{
		#region Fields

		private string _name;
		private byte[] _imageData = null;
		private string _imageUrl;

		#endregion Fields

		#region Constructors

		public Movie ()
		{
		}

		#endregion Constructors

		#region Properties

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public string ImageUrl
		{
			get
			{
				return _imageUrl;
			}
			set
			{
				_imageUrl = value;

				Task.Run (async () =>
				{
					await ImageData();
				});

			}
		}

		#endregion Properties

		#region Methods

		public async System.Threading.Tasks.Task<byte[]> ImageData()
		{
			if (_imageData == null)
			{
				_imageData = await GetImageData();
			}

			return _imageData;
		}

		private async System.Threading.Tasks.Task<byte[]> GetImageData ()
		{
			byte[] result = null;

			//await UIImageExtensions.LoadImage (
			object valueData = CacheHelper.SharedInstance.GetFromCache(ImageUrl);

			if (valueData == null)
			{
				result = await HttpHelper.LoadData(ImageUrl);

				CacheHelper.SharedInstance.AddToCache(ImageUrl, result);
			}
			else
			{
				result = (byte[])valueData;
			}

			return result;
		}

		#endregion Methods
	}
}
