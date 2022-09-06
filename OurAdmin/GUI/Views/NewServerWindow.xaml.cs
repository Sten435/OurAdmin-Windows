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
	/// Interaction logic for NewServerWindow.xaml
	/// </summary>
	public partial class NewServerWindow : Window
	{
		Window MainWindow = new MainWindow();
		public NewServerWindow()
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
				Server server = new(Server.Text, user, Port.Text);

				DomeinController.AddServer(server);

				this.Close();
			} else
			{
				MessageBox.Show("Not all required values are filled in.", "Warning");
			}
		}
	}
}
