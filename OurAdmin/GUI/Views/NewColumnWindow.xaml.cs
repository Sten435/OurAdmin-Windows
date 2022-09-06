using Domein.Controllers;
using Domein.DataBase;
using GUI.ViewModels;
using GUI.Views.ViewClasses;
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
	public partial class NewColumnWindow : Window
	{
		StructureTableViewClass SelectedColumn;
		public NewColumnWindow()
		{
			InitializeComponent();
			this.DataContext = new NewColumnViewModel();
			Validate();
			if (SelectedColumn == null)
				ResetAllFields();
		}

		public NewColumnWindow(StructureTableViewClass selectedColumn) : this()
		{
			SelectedColumn = selectedColumn;
			if (SelectedColumn != null)
				FillDataFields();
			else
			{
				GetWindow(this).Title = "New Column";
				ResetAllFields();
			}
		}

		private void ResetAllFields()
		{
			NewColumnViewModel.OnlyChangeColumn = false;

			NewColumnViewModel.Name = string.Empty;
			NewColumnViewModel.Type = string.Empty;
			NewColumnViewModel.AsDefined = string.Empty;
			NewColumnViewModel.Default = string.Empty;
			NewColumnViewModel.Attribute = string.Empty;
			NewColumnViewModel.AutoIncrement = false;
			NewColumnViewModel.Comments = string.Empty;
			NewColumnViewModel.IsNull = false;
			NewColumnViewModel.LengthValues = string.Empty;
		}

		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		private void Name_TextChanged(object sender, TextChangedEventArgs e)
		{
			Validate();
		}

		private void DataType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Validate();
		}

		private void Default_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = (sender as ComboBox);
			if (comboBox.SelectedValue?.ToString().ToUpper() == "AS DEFINED")
			{
				DefaultData.Text = string.Empty;
				DefaultData.Visibility = Visibility.Visible;
				return;
			}else if(comboBox.SelectedValue?.ToString().ToUpper() == "NULL")
			{
				isNull.IsChecked = true;
			}

			DefaultData.Visibility = Visibility.Collapsed;
			ComboBox_SelectionChanged(sender, e);
		}

		private void FillDataFields()
		{
			NewColumnViewModel.OnlyChangeColumn = true;
			GetWindow(this).Title = $"Change { SelectedColumn.Name}";

			NewColumnViewModel.Name = SelectedColumn.Name ?? string.Empty;
			NewColumnViewModel.Type = SelectedColumn.Column.SqlType?.ToUpper() ?? string.Empty;
			string asDefined = SelectedColumn.Column.__AsDefined?.ToString();
			if (asDefined != null && asDefined.IndexOf("'") == 0 && asDefined.LastIndexOf("'") == asDefined.Length - 1 && asDefined.Length > 2)
			{
				asDefined = asDefined.Remove(asDefined.IndexOf("'"), 1);
				asDefined = asDefined.Remove(asDefined.LastIndexOf("'"), 1);
			}
			NewColumnViewModel.AsDefined = asDefined ?? string.Empty;
			NewColumnViewModel.Default = SelectedColumn.Column.__DefaultValue?.ToString().ToUpper()	 ?? string.Empty;
			NewColumnViewModel.Attribute = SelectedColumn.Column.__Attributes ?? string.Empty;
			NewColumnViewModel.AutoIncrement = SelectedColumn.Column.__AutoIncrement;
			NewColumnViewModel.Comments = SelectedColumn.Column.__Comments;
			NewColumnViewModel.IsNull = bool.Parse(SelectedColumn.Column.IsNull?.ToString());

			int startIndex = SelectedColumn.Type.IndexOf('(');

			if (startIndex != -1)
			{
				string extractedLengthValueFromType = SelectedColumn.Type.Substring(startIndex + 1);
				int lastIndex = extractedLengthValueFromType.IndexOf(')');
				if (lastIndex != -1)
				{
					extractedLengthValueFromType = extractedLengthValueFromType.Remove(lastIndex);
					NewColumnViewModel.LengthValues = SelectedColumn.Column.__LengthValues == string.Empty ? extractedLengthValueFromType : SelectedColumn.Column.__LengthValues;
				}
			}
		}

		private void Validate()
		{
			if (!string.IsNullOrWhiteSpace(Name.Text) && Type.SelectedValue != null)
			{
				AddColumnButton.Opacity = 1;
				AddColumnButton.IsEnabled = true;
				AddColumnButton.Cursor = Cursors.Hand;
				return;
			}
			AddColumnButton.Opacity = 0.4;
			AddColumnButton.IsEnabled = false;
			AddColumnButton.Cursor = Cursors.Hand;
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			if (comboBox.SelectedValue?.ToString() == "NONE")
				comboBox.SelectedValue = string.Empty;
		}
	}
}
