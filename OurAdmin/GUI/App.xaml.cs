using Domein.Controllers;
using Domein.DataBase;
using GUI.Views;
using ReposInterface;
using Repository;
using System;
using System.Diagnostics;
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
		private static DatabaseType databaseType = DatabaseType.MYSQL;

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			//BindingExceptionThrower.Attach()
			ServerRepo = new ServerRepo(databaseType);
			databaseController = new(ServerRepo, databaseType);
			DomeinController.DatabaseController = databaseController;

			MainWindow = new ServerWindow();
			MainWindow.Show();
		}

		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show(e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
			e.Handled = true;
		}
	}
}
