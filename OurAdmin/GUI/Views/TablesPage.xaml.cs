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
		}
	}
}
