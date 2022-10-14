using Domein.DataBase.DataTable;
using GUI.ViewModels;
using GUI.Views.SmallWindows;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace GUI.Views
{
	/// <summary>
	/// Interaction logic for SqlQueryPage.xaml
	/// </summary>
	public partial class SqlQueryPage : UserControl
	{
		private string sortColumn = null;
		GridViewColumnHeader _lastHeaderClicked = null;
		ListSortDirection _lastDirection = ListSortDirection.Ascending;

		public SqlQueryPage()
		{
			InitializeComponent();
			this.DataContext = ViewModelBase.Instance;
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			using var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("GUI.sql.xshd");
			using var reader = new System.Xml.XmlTextReader(stream);
			textEditor.SyntaxHighlighting =
				HighlightingLoader.Load(reader,
				ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
		}

		private void SqlQueryChanged(object sender, EventArgs e)
		{
			ViewModelBase.CustomSqlQueryText = textEditor.Text.Trim();
		}

		private void ExecuteSqlQuery(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textEditor.Text))
				return;
			list.ItemsSource = null;
			gridView.Columns.Clear();

			(List<string> columns, List<List<string>> rows) = ViewModelBase.Instance.ExecuteCustomSqlQuery();
			DataTable dt = new DataTable();

			foreach (string column in columns)
			{
				gridView.Columns.Add(new GridViewColumn() { Header = column, DisplayMemberBinding = new Binding(column) });
				dt.Columns.Add(new DataColumn(column));
			}

			foreach (var item in rows)
			{
				dt.Rows.Add(item.ToArray());
			}

			list.ItemsSource = dt.DefaultView;
		}

		private void ListViewItemClick(object sender, MouseButtonEventArgs e)
		{
			if (list.SelectedItem == null)
				return;
			DataRowView dataRow = list.SelectedValue as DataRowView;
			DataColumnCollection columnList = dataRow.DataView.Table.Columns;

			List<string> row = dataRow.Row.ItemArray.Select(item => item.ToString()).ToList();
			List<string> columns = new();

			foreach (DataColumn column in columnList)
			{
				columns.Add(column.ColumnName.ToString());
			}

			var window = new SqlInfoWindow(columns, row);
			window.Show();
			list.SelectedValue = null;
		}

		private void SortList(object sender, RoutedEventArgs e)
		{
			var headerClicked = e.OriginalSource as GridViewColumnHeader;
			ListSortDirection direction;

			if (headerClicked != null)
			{
				if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
				{
					if (headerClicked != _lastHeaderClicked)
					{
						direction = ListSortDirection.Ascending;
					} else
					{
						if (_lastDirection == ListSortDirection.Ascending)
						{
							direction = ListSortDirection.Descending;
						} else
						{
							direction = ListSortDirection.Ascending;
						}
					}

					var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
					var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

					Sort(sortBy, direction);

					if (direction == ListSortDirection.Ascending)
					{
						headerClicked.Column.HeaderTemplate =
						  Resources["HeaderTemplateArrowUp"] as DataTemplate;
					} else
					{
						headerClicked.Column.HeaderTemplate =
						  Resources["HeaderTemplateArrowDown"] as DataTemplate;
					}

					// Remove arrow from previously sorted header
					if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
					{
						_lastHeaderClicked.Column.HeaderTemplate = null;
					}

					_lastHeaderClicked = headerClicked;
					_lastDirection = direction;
				}
			}
		}

		private void Sort(string sortBy, ListSortDirection direction)
		{
			ICollectionView dataView =
			  CollectionViewSource.GetDefaultView(list.ItemsSource);

			dataView.SortDescriptions.Clear();
			SortDescription sd = new SortDescription(sortBy, direction);
			dataView.SortDescriptions.Add(sd);
			dataView.Refresh();
		}
	}
}
