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

		protected UIView _statusBarView;

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

		protected MovieCategorysTableViewController _tableView 
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
			if (_statusBarView == null)
			{
				_statusBarView = new UIView ()
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.White,
				};
			}
			View.Add (_statusBarView);

			if (_tableView == null)
			{
				_tableView = new MovieCategorysTableViewController();
			}

			View.Add(_tableView.TableView);
		}

		private void AddConstraints()
		{
			View.AddConstraints (new []
			{
				_statusBarView.WithSameTop(View),
				_statusBarView.WithSameLeft(View),
				_statusBarView.WithSameWidth(View),
				_statusBarView.Height().EqualTo(UIApplication.SharedApplication.StatusBarFrame.Height),

				_tableView.TableView.Below (_statusBarView, 0f),
				_tableView.TableView.AtLeftOf (View, 0f),
				_tableView.TableView.AtRightOf (View, 0f),
				_tableView.TableView.AtBottomOf(View, 0f),
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
}