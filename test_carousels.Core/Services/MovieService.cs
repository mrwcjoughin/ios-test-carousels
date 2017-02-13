using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace test_carousels.Core.Services
{
	public interface IMovieService
	{
		Task<MovieResponse> GetMoviesAsync();
	}

	public class MovieService : IMovieService
	{
		private readonly IRestService _restService;

		private const string _url = "http://www.colourlovers.com/api/patterns/random";

		public MovieService (IRestService restService)
		{
			_restService = restService;
		}

		public async Task<MovieResponse> GetMoviesAsync ()
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add("format", (object)"json");

			dynamic result = await _restService.GetAsync<dynamic>(url: _url, parameters: parameters, headerValues: null);
			dynamic firstResult = result[0];
			var description = firstResult["title"];
			var imageUrl = firstResult["imageUrl"];

			return new MovieResponse() { Name = description.Value, ImageUrl = imageUrl.Value };
		}
	}
}
