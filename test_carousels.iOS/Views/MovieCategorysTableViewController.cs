using System;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using test_carousels.Core;
using test_carousels.Core.Models;
using test_carousels.Core.ViewModels;
using UIKit;

namespace test_carousels.iOS
{
	public class MovieCategorysTableViewController : MvxTableViewController
	{
		public new MovieCategorysViewModel ViewModel
		{
			get
			{
				return (MovieCategorysViewModel)base.ViewModel;
			}
			set
			{
				base.ViewModel = value;
			}
		}

		public MovieCategorysTableViewController ()
		{
			this.ViewModel = Mvx.IocConstruct<MovieCategorysViewModel>();

			this.TableView = new UITableView();
			this.TableView.TranslatesAutoresizingMaskIntoConstraints = false;
			this.TableView.BackgroundColor = UIColor.Clear;
			this.TableView.ShowsHorizontalScrollIndicator = false;
			this.TableView.ShowsVerticalScrollIndicator = false;
			this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			this.TableView.RegisterClassForCellReuse (typeof (MovieCategoryViewCell), MovieCategoryViewCell.CellIdentifier);

			this.TableView.Source = new MoviesCategorysViewSource (ViewModel, this.TableView, 250f);
		}
	}

	class MoviesCategorysViewSource : MvxTableViewSource
	{
		private readonly MovieCategorysViewModel _viewModel;

		public event EventHandler OnDecelerationEnded;
		public event EventHandler OnDecelerationStarted;
		public event EventHandler OnDidZoom;
		public event EventHandler OnDraggingStarted;
		public event EventHandler OnScrollAnimationEnded;
		public event EventHandler OnScrolled;
		public event EventHandler OnScrolledToTop;
		private nfloat _rowHeight;

		public MoviesCategorysViewSource (MovieCategorysViewModel viewModel, UITableView tableView, nfloat rowHeight) : base (tableView)
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