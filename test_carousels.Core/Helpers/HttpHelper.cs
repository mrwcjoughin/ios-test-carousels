using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace test_carousels.Core
{
	public static class HttpHelper
	{
		public static async Task<byte[]> LoadData (string url)
		{
			var httpClient = new HttpClient();

			Task<byte[]> contentsTask = httpClient.GetByteArrayAsync (url);

			// await! control returns to the caller and the task continues to run on another thread
			var contents = await contentsTask;

			return contents;
		}
	}
}