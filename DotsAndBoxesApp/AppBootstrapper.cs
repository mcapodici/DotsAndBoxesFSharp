using Caliburn.Micro;
using System.Windows;
using DotsAndBoxesApp.ViewModels;
using System.Collections.Generic;

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
			DisplayRootViewFor<DotsAndBoxesGameViewModel>(new Dictionary<string, object>
			{
				{ "ResizeMode",ResizeMode.NoResize }
			});
		}
	}
}


