﻿using Domein.Validatie;
using GUI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI.Views.Controls
{
	/// <summary>
	/// Interaction logic for SideBar.xaml
	/// </summary>
	public partial class SideBar : UserControl
	{
		Window window;
		public SideBar()
		{
			InitializeComponent();
			this.DataContext = ViewModelBase.Instance;
			NewDatabaseTextChanged(null, null);
		}

		private void AddNewServerPopup(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			NewServerWindow newServer = new();
			newServer.Show();
			window = Window.GetWindow(this);
			window.Close();
		}

		private void NewDatabaseTextChanged(object sender, TextChangedEventArgs e)
		{
			if (ViewModelBase.Instance.IsServerSelected)
			{
				string databaseName = newDatabase.Text.ToLower();

				if (Validate.NullOrWhiteSpace(databaseName))
				{
					newDatabaseButton.IsHitTestVisible = false;
					newDatabaseButton.Opacity = .4;
					return;
				}
				newDatabaseButton.IsHitTestVisible = true;
				newDatabaseButton.Opacity = 1;
				return;
			}
			newDatabaseButton.IsHitTestVisible = false;
			newDatabaseButton.Opacity = .4;
			SelectionChanged();
		}

		private void NewDatabaseClear(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			newDatabase.Text = string.Empty;
		}

		private void NewDatabaseButton(object sender, RoutedEventArgs e)
		{
			newDatabase.Text = string.Empty;
		}

		private void RemoveDatabaseButton(object sender, RoutedEventArgs e)
		{
			SelectionChanged();
		}

		private void SelectionChanged()
		{
			if (!ViewModelBase.Instance.IsDataBaseSelected)
			{
				//removeDatabaseButton.IsHitTestVisible = false;
				//removeDatabaseButton.Opacity = .4;
				return;
			}
			//removeDatabaseButton.IsHitTestVisible = true
			//removeDatabaseButton.Opacity = 1;
		}
	}
}
