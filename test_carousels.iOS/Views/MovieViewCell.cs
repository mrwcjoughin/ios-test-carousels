using System;
using Cirrious.FluentLayouts.Touch;
using Foundation;
using MvvmCross.Core;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using test_carousels.Core.Models;
using test_carousels.iOS.Extension;
using UIKit;

namespace test_carousels.iOS
{
	public class MovieViewCell : MvxCollectionViewCell
	{
		#region Fields

		public const string CellIdentifier = "MovieViewCell";

		private Movie _movie;

		private UIButton _nameLabel;
		private UIButton _movieImageView;
		private UIButton _infoImageView;
		private UIButton _playImageView;

		private bool _imageSet = false;

		#endregion Fields

		#region Properties

		#endregion Properties

		#region Constructors

		public MovieViewCell(IntPtr handle) 
			: base(handle)
		{
			//TranslatesAutoresizingMaskIntoConstraints = false;
			BackgroundColor = UIColorExtension.LightBlack;
		}

		#endregion Constructors

		#region Methods

		private void AddControls()
		{
			if (_nameLabel == null)
			{
				_nameLabel = new UIButton
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.Clear,
					Font = UIFont.FromName (Constants.MainFont, 15f),
				};
				_nameLabel.TitleLabel.TextAlignment = UITextAlignment.Left;
				_nameLabel.TitleLabel.TextColor = UIColor.White;
				_nameLabel.TitleLabel.Lines = 1;
				_nameLabel.TitleLabel.LineBreakMode = UILineBreakMode.TailTruncation;
				ContentView.Add(_nameLabel);
				_nameLabel.UserInteractionEnabled = true;
				_nameLabel.TouchUpInside += Any_TouchUpInside;
			}

			if (_movieImageView == null)
			{
				_movieImageView = new UIButton
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.Clear,
					ContentMode = UIViewContentMode.ScaleAspectFit,
				};
				ContentView.Add(_movieImageView);
				_movieImageView.UserInteractionEnabled = true;
				//_movieImageView.AddGestureRecognizer(tapGestureRecognizer);
				_movieImageView.TouchUpInside += Any_TouchUpInside;
			}

			if (_playImageView == null)
			{
				_playImageView = new UIButton
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.Clear,
					ContentMode = UIViewContentMode.ScaleAspectFit,

				};
				_playImageView.SetImage(UIImage.FromBundle("play.png"), UIControlState.Normal);
				_playImageView.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				ContentView.Add(_playImageView);
				_playImageView.UserInteractionEnabled = true;
				_playImageView.TouchUpInside += Any_TouchUpInside;
			}

			if (_infoImageView == null)
			{
				_infoImageView = new UIButton
				{
					TranslatesAutoresizingMaskIntoConstraints = false,
					BackgroundColor = UIColor.Clear,
					ContentMode = UIViewContentMode.ScaleAspectFit,
					//Image = UIImage.FromBundle("info.png"),
				};
				_infoImageView.SetImage(UIImage.FromBundle("info.png"), UIControlState.Normal);
				_infoImageView.ImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				ContentView.Add(_infoImageView);
				_infoImageView.UserInteractionEnabled = true;
				_infoImageView.TouchUpInside += Any_TouchUpInside;
			}
		}

		private void AddConstraints()
		{
			ContentView.RemoveConstraints(ContentView.Constraints);

			ContentView.AddConstraints (new []
			{
				_movieImageView.AtTopOf (ContentView, 0f),
				_movieImageView.AtLeftOf (ContentView, 0f),
				_movieImageView.Width().EqualTo (Constants.CellWidth),
				_movieImageView.Height().EqualTo (Constants.CellHeightLessTopAndBottomMargins - 20f),

				_playImageView.WithSameCenterX (_movieImageView),
				_playImageView.WithSameCenterY (_movieImageView),
				_playImageView.Width().EqualTo (50f),
				_playImageView.Height().EqualTo (50f),

				_nameLabel.AtLeftOf (ContentView, Constants.HorizontalMargin),
				_nameLabel.Height().EqualTo (20f),
				_nameLabel.AtBottomOf (ContentView, Constants.VerticalMargin),

				_infoImageView.AtRightOf (ContentView, Constants.HorizontalMargin),
				_infoImageView.Width().EqualTo (30f),
				_infoImageView.Height().EqualTo (30f),
				_infoImageView.AtBottomOf (ContentView, Constants.VerticalMargin),
			});
		}

		public void SetupCell (Movie movie)
		{
			_movie = movie;

			AddControls();
			AddConstraints();

			_nameLabel.SetTitle(_movie.Name, UIControlState.Normal);

			InvokeOnMainThread (async () =>
			{
				try
				{
					if (!_imageSet)
					{
						var imageData = await _movie.ImageData();

						_movieImageView.SetImage(UIImage.LoadFromData (NSData.FromArray (imageData)), UIControlState.Normal);
						_movieImageView.ImageView.ContentMode = UIViewContentMode.ScaleToFill;
						_imageSet = true;
					}
				}
				catch (Exception ex)
				{
					System.Console.WriteLine (ex.ToString ());
				}
			});
		}

		private void Any_TouchUpInside(object sender, EventArgs e)
		{
			ShowMessage("Alert", _movie.Name);
		}

		protected void ShowMessage(string heading, string message)
		{
			UIAlertView alert = new UIAlertView () 
			{
				Title = heading,
				Message = message,
			};
			alert.AddButton("OK");
			alert.Show ();
		}

		#endregion Methods
	}
}