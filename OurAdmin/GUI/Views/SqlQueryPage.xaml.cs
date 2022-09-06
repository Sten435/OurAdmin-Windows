using Domein.DataBase.DataTable;
using GUI.ViewModels;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
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
			list.ItemsSource = null;
			gridView.Columns.Clear();

			(List<string> columns, List<string> rows) = ViewModelBase.Instance.ExecuteCustomSqlQuery;
			DataTable dt = new DataTable();

			foreach (string column in columns)
			{
				gridView.Columns.Add(new GridViewColumn() { Header = column, DisplayMemberBinding = new Binding(column) });
				dt.Columns.Add(new DataColumn(column));
			}

			for (int i = 0; i < rows.ToArray().Length; i++)
			{
				dt.Rows.Add(rows.ToArray()[i]);
			}

			list.ItemsSource = dt.DefaultView;
		}

		private void list_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
		{

		}
	}
}
