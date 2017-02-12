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
		private MoviesViewModel _viewModel;

		public MoviesViewModel ViewModel
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

		public MoviesCollectionView (CGRect frame, UICollectionViewLayout layout, MoviesViewModel viewModel)
			: base(frame, layout)
		{
			this.ViewModel = viewModel;

			//this. = new UITableView();
			TranslatesAutoresizingMaskIntoConstraints = false;
			BackgroundColor = UIColor.Green;
			//ShowsHorizontalScrollIndicator = false;
			//ShowsVerticalScrollIndicator = false;
			//this.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			this.RegisterClassForCell (typeof (MovieViewCell), MovieViewCell.CellIdentifier);

			var source = new MoviesViewSource (ViewModel, this, 250f);

			this.Source = source;

			this.ReloadData();
		}

		//protected int NumberOfSectionsInCollectionView(UICollectionView collectionView)
		//{
		//	return 1;
		//}

		//protected int CollectionView(UICollectionView collectionView, int section) //NumberOfItemsInSection
		//{
		//	return _viewModel.Movies.Count;
		//}

		//protected UICollectionViewCell CollectionView(UICollectionView collectionView, NSIndexPath indexPath)
		//{
		//	var cell = collectionView.DequeueReusableCellWithReuseIdentifier(reuseIdentifier, forIndexPath: indexPath);
		//	return cell
		//}
	}

	class MoviesViewSource : MvxCollectionViewSource
	{
		private readonly MoviesViewModel _viewModel;

		public event EventHandler OnDecelerationEnded;
		public event EventHandler OnDecelerationStarted;
		public event EventHandler OnDidZoom;
		public event EventHandler OnDraggingStarted;
		public event EventHandler OnScrollAnimationEnded;
		public event EventHandler OnScrolled;
		public event EventHandler OnScrolledToTop;
		//private nfloat _rowHeight;

		public MoviesViewSource (MoviesViewModel viewModel, UICollectionView collectionView, nfloat rowHeight) 
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
			return _viewModel != null ? _viewModel.Movies.Count : 0;
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
					movie = _viewModel.Movies[indexPath.Row];
				}

					//cell.DataContext = movie;

				cell.SetupCell (movie);	
				//}
			}

			cell.Frame = new CGRect(cell.Frame.X, cell.Frame.Y, 300f, 200f);
				
			return cell;
		}

		public override void WillDisplayCell (UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
		{
			var cellz = cell as MovieViewCell;
			//cellz.ClearAllBindings ();
			//cellz.BindUrl ();
		}

		//public override nfloat GetHeightForRow (UICollectionView collectionView, NSIndexPath indexPath)
		//{
		//	return _rowHeight;
		//}

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