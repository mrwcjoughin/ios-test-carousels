using System;
using System.Drawing;
using Cirrious.FluentLayouts.Touch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using test_carousels.Core.Models;
using UIKit;

namespace test_carousels.iOS
{
	public class MovieCategoryViewCell : MvxTableViewCell
	{
		#region Fields

		public const string CellIdentifier = "MovieCategoryViewCell";

		private UILabel _nameLabel;
		private MoviesCollectionView _moviesCollectionView;

		#endregion Fields

		#region Properties

		public MovieCategory ViewModel
		{
			get
			{
				return (MovieCategory)this.DataContext;
			}
		}

		#endregion Properties

		#region Constructors

		public MovieCategoryViewCell (IntPtr handle) 
			: base(handle)
		{
		}

		#endregion Constructors

		#region Methods

		private void AddControls()
		{
			BackgroundColor = UIColor.Clear;

			if (_nameLabel == null)
			{
				_nameLabel = new UILabel
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.Clear,
					Font = UIFont.FromName (Constants.MainFont, 25f),
					Text = "Test",
					TextColor = UIColor.White,
				};
				ContentView.Add(_nameLabel);
			}

			if (_moviesCollectionView == null)
			{
				var uiCollectionViewFlowLayout = new UICollectionViewFlowLayout()
				{
					//SectionInset = new UIEdgeInsets(20, 5, 5, 5),
					//MinimumInteritemSpacing = 5,
					//MinimumLineSpacing = 5,
					ItemSize = new SizeF((float)Constants.CellWidth, (float)Constants.CellHeight),
					ScrollDirection = UICollectionViewScrollDirection.Horizontal,
				};

				_moviesCollectionView = new MoviesCollectionView(new CoreGraphics.CGRect(0f, 0f, Constants.WidthLessLeftMargins, 200f), uiCollectionViewFlowLayout, ViewModel);
				ContentView.Add(_moviesCollectionView);
			}
		}

		private void AddConstraints()
		{
			ContentView.RemoveConstraints(ContentView.Constraints);

			ContentView.AddConstraints (new []
			{
				_nameLabel.AtTopOf (ContentView, Constants.VerticalMargin),
				_nameLabel.AtLeftOf (ContentView, Constants.HorizontalMargin),
				_nameLabel.Width().EqualTo (Constants.WidthLessLeftMargins),
				_nameLabel.Height().EqualTo (30f),
				//_nameLabel.AtBottomOf (ContentView, Constants.VerticalMargin),

				_moviesCollectionView.Below (_nameLabel, Constants.VerticalMargin),
				_moviesCollectionView.AtLeftOf (ContentView, Constants.HorizontalMargin),
				_moviesCollectionView.Width().EqualTo (Constants.WidthLessLeftMargins),
				_moviesCollectionView.Height().EqualTo (200f),
			});
		}

		public void SetupCell ()
		{
			AddControls();
			AddConstraints();
			_nameLabel.Text = ViewModel.Name;
		}

		#endregion Methods
	}
}