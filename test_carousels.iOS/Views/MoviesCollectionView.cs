using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.iOS.Views;
using test_carousels.Core;
using test_carousels.Core.Models;
using test_carousels.Core.ViewModels;
using UIKit;

namespace test_carousels.iOS
{
	public class MoviesCollectionView : UICollectionView
	{
		private MovieCategory _viewModel;
		System.Threading.Timer _timer = null;

		public MovieCategory ViewModel
		{
			get
			{
				return _viewModel;
			}
			set
			{
				_viewModel = value;
			}
		}

		public MoviesCollectionView (CGRect frame, UICollectionViewLayout layout, MovieCategory viewModel)
			: base(frame, layout)
		{
			this.ViewModel = viewModel;

			//this. = new UITableView();
			TranslatesAutoresizingMaskIntoConstraints = false;
			BackgroundColor = UIColor.Clear;

			ShowsHorizontalScrollIndicator = true;
			ShowsVerticalScrollIndicator = false;
			//this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			UserInteractionEnabled = true;

			this.RegisterClassForCell (typeof (MovieViewCell), MovieViewCell.CellIdentifier);

			var source = new MoviesViewSource (ViewModel, this, 250f);

			this.Source = source;

			_timer = new System.Threading.Timer((obj) =>
			{
				//_timer.Dispose();

				InvokeOnMainThread ( () => {
					this.ReloadData();
				});
			}, null, 150, 500);
		}
	}

	class MoviesViewSource : MvxCollectionViewSource
	{
		private readonly MovieCategory _viewModel;

		public event EventHandler OnDecelerationEnded;
		public event EventHandler OnDecelerationStarted;
		public event EventHandler OnDidZoom;
		public event EventHandler OnDraggingStarted;
		public event EventHandler OnScrollAnimationEnded;
		public event EventHandler OnScrolled;
		public event EventHandler OnScrolledToTop;
		//private nfloat _rowHeight;

		public MoviesViewSource (MovieCategory viewModel, UICollectionView collectionView, nfloat rowHeight) 
			: base (collectionView)
		{
			_viewModel = viewModel;
			//_rowHeight = rowHeight;
		}

		public override nint NumberOfSections(UICollectionView collectionView)
		{
			return 1;
		}

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
		{
			return _viewModel != null ? _viewModel.Movies.Movies.Count : 0;
		}

		public override Boolean ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
		{
			return false;
		}

		protected override UICollectionViewCell GetOrCreateCellFor (UICollectionView collectionView, NSIndexPath indexPath, object item)
		{
			var cell = (MovieViewCell)collectionView.DequeueReusableCell (MovieViewCell.CellIdentifier, indexPath);

			if (cell != null) {
				//if (cell.DataContext == null)
				//{
				var movie = (Movie)item;

				if (movie == null)
				{
					movie = _viewModel.Movies.Movies[indexPath.Row];
				}

				//cell.DataContext = movie;

				cell.SetupCell (movie);	
			}
				
			return cell;
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