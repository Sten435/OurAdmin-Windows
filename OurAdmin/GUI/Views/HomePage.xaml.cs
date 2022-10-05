using GUI.ViewModels;
using GUI.Views.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
			this.DataContext = ViewModelBase.Instance;
		}
	}
}
