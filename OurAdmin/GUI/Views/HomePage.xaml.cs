using GUI.ViewModels;
using System;
using System.Windows.Controls;

namespace GUI.Views
{
	/// <summary>
	/// Interaction logic for TablesView.xaml
	/// </summary>
	public partial class HomePage : Page
	{
		public HomePage()
		{
			InitializeComponent();
			this.DataContext = new ViewModelBase();
		}
	}
}
