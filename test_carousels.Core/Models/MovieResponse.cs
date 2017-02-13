using System;
using Newtonsoft.Json;

namespace test_carousels.Core
{
	public class MovieResponse
	{
		[JsonProperty("description")]
		public string Name 
		{
			get; 
			set;
		}

		[JsonProperty("imageUrl")]
		public string ImageUrl
		{
			get; 
			set;
		}
	}
}