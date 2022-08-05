using Domein.Controllers;
using Domein.DataBase;
using ReposInterface;
using Repository;
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

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			ServerRepo = new ServerRepo(DatabaseType.MYSQL);
			databaseController = new(ServerRepo);
			DomeinController.DatabaseController = databaseController;

			Server server = new("localhost", new(user: "root", password: ""));
			Database database = new("testdatabase");
			DomeinController.AddServer(server);
			DomeinController.OpenConnectionToServer(server);
			DomeinController.AddDatabase(database);
			DomeinController.UseDatabase(database);

			MainWindow = new MainWindow();
			MainWindow.Show();
		}

		private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning);
			e.Handled = true;
		}

	}
}
