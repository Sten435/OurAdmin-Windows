using Domein.Controllers;
using GUI.ViewModels;
using System;
using System.Data;
using System.Windows;
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
			this.DataContext = ViewModelBase.Instance;
			newTable_TextChanged(null, null);
		}

		private void newTable_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(newTable.Text))
			{
				goBtn.IsEnabled = false;
				goBtn.Opacity = 0.4;
				return;
			}
			goBtn.IsEnabled = true;
			goBtn.Opacity = 1;
		}
	}
}
