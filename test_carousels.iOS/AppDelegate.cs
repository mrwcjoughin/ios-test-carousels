using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.Platform;
using Foundation;
using UIKit;

namespace test_carousels.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : MvxApplicationDelegate
	{
		public override UIWindow Window
		{
			get;
			set;
		}

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			Window = new UIWindow (UIScreen.MainScreen.Bounds);

			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			var setup = new Setup (this, Window);
			setup.Initialize ();

			var startup = Mvx.Resolve<IMvxAppStart> ();
			startup.Start ();

			Window.MakeKeyAndVisible ();

			return true;
		}
	}
}
