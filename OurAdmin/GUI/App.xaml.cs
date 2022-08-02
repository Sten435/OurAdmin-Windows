using Domein.Controllers;
using ReposInterface;
using Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace GUI
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static IServerInfo ServerRepo;
		private static DatabaseController databaseController;
		private static DomeinController domeinController;

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			ServerRepo = new ServerRepo(DatabaseType.MYSQL);
			databaseController = new(ServerRepo);
			domeinController = new(databaseController);

			MainWindow = new MainWindow(domeinController);
			MainWindow.Show();
		}

		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
			e.Handled = true;
		}

	}
}
