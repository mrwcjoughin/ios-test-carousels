using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmCross.Core.ViewModels;
using test_carousels.Core.Models;

namespace test_carousels.Core
{
	public class MoviesViewModel : MvxViewModel
	{
		#region Fields

		private ObservableCollection<Movie> _movies = new ObservableCollection<Movie>();

		#endregion Fields

		#region Constructors

		public MoviesViewModel(IMovieManager movieManager)
		{
			Task.Run (async () =>
			{
				var max = 10;

				for (int i = 0; i < max; i++)
				{
					try
					{
						var result = await movieManager.GetMoviesAsync();

						_movies.Add(new Movie() { Name = result.Name, ImageUrl = result.ImageUrl });
					}
					catch(Exception ex)
					{
						//Do nothing intentially
					}
				}
			});
		}

		#endregion Constructors

		#region Properties

		public ObservableCollection<Movie> Movies
		{
			get
			{
				return _movies;
			}
			set
			{
				SetProperty(ref _movies, value);
			}
		}

		#endregion Properties
	}
}