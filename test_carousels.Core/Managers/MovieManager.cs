using System;
using System.Threading.Tasks;
using test_carousels.Core.Services;

namespace test_carousels.Core
{
	public interface IMovieManager
	{
		Task<MovieResponse> GetMoviesAsync();
	}

	public class MovieManager : IMovieManager
	{
		private readonly IMovieService _movieService;

		public MovieManager(IMovieService movieService)
		{
			_movieService = movieService;
		}

		public async Task<MovieResponse> GetMoviesAsync ()
		{
			return await _movieService.GetMoviesAsync();
		}
	}
}
