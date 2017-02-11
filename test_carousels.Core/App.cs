using MvvmCross.Platform.IoC;

namespace test_carousels.Core
{
	public class App : MvvmCross.Core.ViewModels.MvxApplication
	{
		public override void Initialize ()
		{
			CreatableTypes ()
				.EndingWith ("Service")
				.AsInterfaces ()
				.RegisterAsLazySingleton ();

			RegisterAppStart<ViewModels.MainViewModel> ();
		}
	}
}