using System.Collections.Generic;
using System.Collections.ObjectModel;
using MvvmCross.Core.ViewModels;
using test_carousels.Core.Models;

namespace test_carousels.Core.ViewModels
{
	public class MainViewModel
		: MvxViewModel
	{
		#region Fields

		private ObservableCollection<MovieCategory> _movieCategorys;

		#endregion Fields

		#region Properties

		public ObservableCollection<MovieCategory> MovieCategorys
		{
			get
			{
				return _movieCategorys;
			}
			set
			{
				SetProperty(ref _movieCategorys, value);
			}
		}

		#endregion Properties
	}
}
