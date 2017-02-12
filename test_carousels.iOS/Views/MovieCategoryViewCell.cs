using System;
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
			AddControls();
			AddConstraints();
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

				//_clicksPointsSeparatorView.AtBottomOf (ContentView, 0f),
				//_clicksPointsSeparatorView.WithSameWidth (ContentView),
				//_clicksPointsSeparatorView.Height().EqualTo (1f),
			});
		}

		private void AddBinding()
		{
			var set = this.CreateBindingSet<MovieCategoryViewCell, MovieCategory> ();
			set.Bind (_nameLabel).To (vm => vm.Name);
			//set.Bind (TextField).To (vm => vm.Hello);
			set.Apply ();
		}

		public void SetupCell ()
		{
			//AddBinding();
			_nameLabel.Text = ViewModel.Name;
		}

		#endregion Methods
	}
}