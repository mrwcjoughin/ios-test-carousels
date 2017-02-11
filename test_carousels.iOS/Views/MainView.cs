using System;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using test_carousels.Core.Models;
using test_carousels.Core.ViewModels;
using UIKit;

namespace test_carousels.iOS.Views
{
	public class MainView : MvxViewController
	{
		#region Fields

		//private readonly MainViewModel _viewModel;
		//private UILabel _test;

		#endregion Fields

		#region Constructors

		//public MainView (IntPtr handle) : base (handle)
		//{
		//}

		#endregion Constructors

		#region Properties

		public new MainViewModel ViewModel
		{
			get
			{
				return (MainViewModel)base.ViewModel;
			}
			//set
			//{
			//	base.ViewModel = value;
			//}
		}

		protected UITableView _tableView 
		{ 
			get; 
			set; 
		}

		#endregion Properties

		#region Methods

		public override void ViewDidAppear (bool animated)
		{
			//StoreLocatorView.CurrentStoreLocatorView = this;

			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			NavigationController.SetNavigationBarHidden (true, false);

			base.ViewDidAppear (animated);

			NavigationController.SetNavigationBarHidden (true, false);

			NavigationController.NavigationBarHidden = true;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			this.View.BackgroundColor = UIColor.Black;

			AddControls();
			AddConstraints();
			AddBinding();
		}

		private void AddControls()
		{
			//_test = new UILabel()
			//{
			//	TranslatesAutoresizingMaskIntoConstraints = false,
			//	TextColor = UIColor.White,
			//};
			//View.Add(_test);

			_tableView = new UITableView
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				BackgroundColor = UIColor.Clear,
				ShowsHorizontalScrollIndicator = false,
				ShowsVerticalScrollIndicator = false,
				SeparatorStyle = UITableViewCellSeparatorStyle.None,
			};

			_tableView.RegisterClassForCellReuse (typeof (MovieCategoryViewCell), MovieCategoryViewCell.CellIdentifier);

			_tableView.Source = new MoviesCategorysViewSource (ViewModel, _tableView, 50f);

			View.Add(_tableView);
		}

		private void AddConstraints()
		{
			View.AddConstraints (new []
			{
				_tableView.AtTopOf (View, 0f),
				_tableView.AtLeftOf (View, 0f),
				_tableView.AtRightOf (View, 0f),
				_tableView.AtBottomOf(View, 0f),
				//_test.Width().EqualTo(Constants.WidthLessLeftAndRightMargins),
			});
		}

		private void AddBinding()
		{
			var set = this.CreateBindingSet<MainView, MainViewModel> ();
			//set.Bind (_test).To (vm => vm.ApplicationTitle);
			//set.Bind (TextField).To (vm => vm.Hello);
			set.Apply ();
		}

		#endregion Methods
	}

	class MoviesCategorysViewSource : MvxTableViewSource
	{
		private readonly MainViewModel _viewModel;

		public event EventHandler OnDecelerationEnded;
		public event EventHandler OnDecelerationStarted;
		public event EventHandler OnDidZoom;
		public event EventHandler OnDraggingStarted;
		public event EventHandler OnScrollAnimationEnded;
		public event EventHandler OnScrolled;
		public event EventHandler OnScrolledToTop;
		private nfloat _rowHeight;

		public MoviesCategorysViewSource (MainViewModel viewModel, UITableView tableView, nfloat rowHeight) : base (tableView)
		{
			_viewModel = viewModel;
			_rowHeight = rowHeight;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return _viewModel.MovieCategorys != null ? _viewModel.MovieCategorys.Count : 0;
		}

		protected override UITableViewCell GetOrCreateCellFor (UITableView tableView, NSIndexPath indexPath, object item)
		{
			var cell =  (MovieCategoryViewCell)tableView.DequeueReusableCell (MovieCategoryViewCell.CellIdentifier, indexPath);

			if (cell != null) {
				if (cell.DataContext == null)
				{
					var movieCategory = (MovieCategory)item;

					if (movieCategory == null)
					{
						movieCategory = _viewModel.MovieCategorys[indexPath.Row];
					}

					cell.DataContext = movieCategory;

					cell.SetupCell ();
				}
			}

			return cell;
		}

		public override nfloat GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return _rowHeight;
		}

		public override void DecelerationEnded (UIScrollView scrollView)
		{
			if (OnDecelerationEnded != null)
			{
				OnDecelerationEnded (scrollView, null);
			}
		}

		public override void DecelerationStarted (UIScrollView scrollView)
		{
			if (OnDecelerationStarted != null)
			{
				OnDecelerationStarted (scrollView, null);
			}
		}

		public override void DidZoom (UIScrollView scrollView)
		{
			if (OnDidZoom != null)
			{
				OnDidZoom (scrollView, null);
			}
		}

		public override void DraggingStarted (UIScrollView scrollView)
		{
			if (OnDraggingStarted != null)
			{
				OnDraggingStarted (scrollView, null);
			}
		}

		public override void ScrollAnimationEnded (UIScrollView scrollView)
		{
			if (OnScrollAnimationEnded != null)
			{
				OnScrollAnimationEnded (scrollView, null);
			}
		}

		public override void Scrolled (UIScrollView scrollView)
		{
			if (OnScrolled != null)
			{
				//MvvmCross.Platform.Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "Scrolled");
				OnScrolled (scrollView, null);
			}
		}

		public override void ScrolledToTop (UIScrollView scrollView)
		{
			if (OnScrolledToTop != null)
			{
				OnScrolledToTop (scrollView, null);
			}
		}
	}
}