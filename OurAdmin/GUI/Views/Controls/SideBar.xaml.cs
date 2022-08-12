using GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace GUI.Views.Controls
{
	/// <summary>
	/// Interaction logic for SideBar.xaml
	/// </summary>
	public partial class SideBar : UserControl
	{
		public SideBar()
		{
			InitializeComponent();
			this.DataContext = ViewModelBase.Instance;
		}

		private void TextBlock_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NewServerWindow newServer = new();
			newServer.Show();
			var myWindow = Window.GetWindow(this);
			myWindow.Close();
		}
	}
}
