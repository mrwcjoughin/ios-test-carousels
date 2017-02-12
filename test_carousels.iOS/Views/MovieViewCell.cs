using System;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using test_carousels.Core.Models;
using UIKit;

namespace test_carousels.iOS
{
	public class MovieViewCell : MvxCollectionViewCell // UICollectionViewCell
	{
		#region Fields

		public const string CellIdentifier = "MovieViewCell";

		private Movie _movie;

		private UILabel _nameLabel;

		#endregion Fields

		#region Properties

		//public Movie ViewModel
		//{
		//	get
		//	{
		//		return (Movie)this.DataContext;
		//	}
		//}

		//public Movie ViewModel
		//{
		//	get
		//	{
		//		return (Movie)this.DataContext;
		//	}
		//}

		#endregion Properties

		#region Constructors

		public MovieViewCell(IntPtr handle) 
			: base(handle)
		{
			BackgroundColor = UIColor.Blue;
			AddControls();
			AddConstraints();
		}

		#endregion Constructors

		#region Methods

		private void AddControls()
		{
			BackgroundColor = UIColor.Gray;

			if (_nameLabel == null)
			{
				_nameLabel = new UILabel
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.Clear,
					Font = UIFont.FromName (Constants.MainFont, 15f),
					Text = "Test",
					TextColor = UIColor.White,
					Lines = 0,
					LineBreakMode = UILineBreakMode.WordWrap,
				};
				ContentView.Add(_nameLabel);
			}
		}

		private void AddConstraints()
		{
			ContentView.RemoveConstraints(ContentView.Constraints);

			ContentView.AddConstraints (new []
			{
				//_nameLabel.AtTopOf (ContentView, Constants.VerticalMargin),
				_nameLabel.AtLeftOf (ContentView, Constants.HorizontalMargin),
				//_nameLabel.Width().EqualTo (Constants.WidthLessLeftMargins),
				_nameLabel.WithSameWidth(ContentView),
				_nameLabel.Height().EqualTo (30f),
				_nameLabel.AtBottomOf (ContentView, Constants.VerticalMargin),

				//_clicksPointsSeparatorView.AtBottomOf (ContentView, 0f),
				//_clicksPointsSeparatorView.WithSameWidth (ContentView),
				//_clicksPointsSeparatorView.Height().EqualTo (1f),
			});
		}

		//private void AddBinding()
		//{
		//	var set = this.CreateBindingSet<MovieViewCell, Movie> ();
		//	set.Bind (_nameLabel).To (vm => vm.Name);
		//	//set.Bind (TextField).To (vm => vm.Hello);
		//	set.Apply ();
		//}

		public void SetupCell (Movie movie)
		{
			_movie = movie;
			//AddBinding();
			_nameLabel.Text = _movie.Name;
		}

		#endregion Methods
	}
}