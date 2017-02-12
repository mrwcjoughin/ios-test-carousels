using System;
using System.Collections.ObjectModel;
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

		public MoviesViewModel()
		{
			_movies.Add(new Movie() { Name = "Back to the Future", ImageUrl = "https://images-na.ssl-images-amazon.com/images/M/MV5BZmU0M2Y1OGUtZjIxNi00ZjBkLTg1MjgtOWIyNThiZWIwYjRiXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_SY1000_CR0,0,643,1000_AL_.jpg" });
			_movies.Add(new Movie() { Name = "Back to the Future II", ImageUrl = "https://images-na.ssl-images-amazon.com/images/M/MV5BZTMxMGM5MjItNDJhNy00MWI2LWJlZWMtOWFhMjI5ZTQwMWM3XkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_.jpg" });
			_movies.Add(new Movie() { Name = "Back to the Future III", ImageUrl = "https://images-na.ssl-images-amazon.com/images/M/MV5BYjhlMGYxNmMtOWFmMi00Y2M2LWE5NWYtZTdlMDRlMGEzMDA3XkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_SY1000_CR0,0,676,1000_AL_.jpg" });
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