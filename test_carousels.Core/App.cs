using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using test_carousels.Core.Services;

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

			CreatableTypes()
				.EndingWith("Manager")
				.AsInterfaces()
				.RegisterAsLazySingleton();
			
			Mvx.RegisterSingleton<IModernHttpClient>(() => new test_carousels.Core.ModernHttpClient());
			Mvx.ConstructAndRegisterSingleton<IMovieService, MovieService>();
			Mvx.ConstructAndRegisterSingleton<IMovieManager, MovieManager>();

			RegisterAppStart<ViewModels.MainViewModel> ();
		}
	}
}