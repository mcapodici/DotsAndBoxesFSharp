using Caliburn.Micro;
using System.Windows;
using DotsAndBoxesApp.ViewModels;

namespace DotsAndBoxesApp
{
	class AppBootstrapper : BootstrapperBase
	{
		public AppBootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<DotsAndBoxesGameViewModel>();
		}
	}
}


