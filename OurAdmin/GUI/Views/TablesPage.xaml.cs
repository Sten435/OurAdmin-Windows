using GUI.ViewModels.Pages;
using System.Windows.Controls;

namespace GUI.Views
{
	/// <summary>
	/// Interaction logic for TablesPage.xaml
	/// </summary>
	public partial class TablesPage : UserControl
	{
		public TablesPage()
		{
			InitializeComponent();
			this.DataContext = new TablesViewModel();
		}
	}
}
