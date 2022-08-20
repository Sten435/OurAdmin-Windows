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
				ResetAllFields();
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
			if ((sender as ComboBox).SelectedValue.ToString().ToUpper() == "AS DEFINED")
			{
				DefaultData.Visibility = Visibility.Visible;
				return;
			}
			DefaultData.Visibility = Visibility.Collapsed;

		}

		private void FillDataFields()
		{
			NewColumnViewModel.OnlyChangeColumn = true;

			NewColumnViewModel.Name = SelectedColumn.Name!;
			NewColumnViewModel.Type = SelectedColumn.Column.SqlType.ToUpper()!;
			NewColumnViewModel.AsDefined = SelectedColumn.Column.__AsDefined!.ToString();
			NewColumnViewModel.Default = SelectedColumn.Column.__DefaultValue?.ToString();
			NewColumnViewModel.Attribute = SelectedColumn.Column.__Attributes;
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
	}
}
