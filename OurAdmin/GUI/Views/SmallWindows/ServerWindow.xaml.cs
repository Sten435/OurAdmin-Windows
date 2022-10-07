using Domein.Controllers;
using Domein.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI.Views
{
	/// <summary>
	/// Interaction logic for ServerWindow.xaml
	/// </summary>
	public partial class ServerWindow : Window
	{
		Window MainWindow = new MainWindow();
		public ServerWindow()
		{
			InitializeComponent();
		}

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			int serverAmount = DomeinController.GetServers().Count;
			if (serverAmount > 0)
			{
				try
				{
					HomePage homePage = new HomePage();
					MainWindow.Content = homePage;
					MainWindow.Show();
				} catch (Exception exception)
				{
					throw new Exception(exception.Message);
				}
			}
		}

		private void AddServer(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(Server.Text) &&
				!string.IsNullOrWhiteSpace(User.Text) &&
				!string.IsNullOrWhiteSpace(Port.Text))
			{
				UserInfo user = new(User.Text, Password.Text);
				Server server = new(Server.Text.Trim(), user, Port.Text);

				if (DomeinController.CheckConnectionToServer(server))
					DomeinController.AddServer(server);
				else
				{
					MessageBox.Show($"Check cridentials and try again.\n\nHost: {server.Host}\nUser: {user.User}\nPassword: {user.Password}\nPort: {server.Port}", "Can't connect to host", MessageBoxButton.OK);
					return;
				}

				this.Close();
			} else
			{
				MessageBox.Show("Not all required values are filled in.", "Warning");
			}
		}
	}
}
