using Domein.Controllers;
using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Domein.Validatie;
using GUI.Views;
using GUI.Views.SmallWindows;
using GUI.Views.ViewClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace GUI.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		private static ViewModelBase instance = null;
		private static readonly object padlock = new object();

		public ViewModelBase()
		{
			if (!DomeinController.IsDatabaseConnected)
				return;
		}


		public static ViewModelBase Instance {
			get {
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new ViewModelBase();
					}
					return instance;
				}
			}
		}

		public delegate void NotifyDatabaseSelectionChangedType();
		public event NotifyDatabaseSelectionChangedType NotifyDatabaseSelectionChanged;

		public List<string> Tables {
			get {
				List<string> _tables = new(DomeinController.GetTables());
				if (DomeinController.IsDatabaseConnected)
					if (_tables.Count == 0)
						return new() { "NO TABLES FOUND" };
				return _tables;
			}
		}

		public List<string> StructuurTypes {
			get {
				if (IsServerConnected)
					return DomeinController.GetServerTypes();
				return new();
			}
		}
		public List<Server> ServerList {
			get {
				try
				{
					if (AnyServersAvailable)
					{
						return new(DomeinController.GetServers().OrderBy(server => server.Host.Length));
					}
					IsServerSelected = false;
					SelectedServer = null;
					OnPropertyChanged(nameof(SelectedServer));
					OnPropertyChanged(nameof(IsServerSelected));
					return new();
				} catch (Exception)
				{
					return new();
				}
			}
		}

		public static string NewTableToAdd { get; set; }
		public static string NewDatabaseToAdd { get; set; }
		public static string NewColumnName { get; set; }
		public static string CustomSqlQueryText { get; set; }

		public List<Database> DatabaseList {
			get {
				try
				{
					List<Database> result = new List<Database>(DomeinController.GetDatabasesFromServer().OrderBy(database => database.Name.Length));
					for (int i = 0; i < result.Count; i++)
					{
						DomeinController.AddDatabase(result[i]);
					}
					return result;
				} catch (Exception error)
				{
					if (IsServerConnected)
					{
						DomeinController.CloseConnectionToServer();
						DomeinController.RemoveServer(DomeinController.GetServers().Find(server => server.Host == SelectedServer.Host));
						OnPropertyChanged(nameof(ServerList));
						MessageBox.Show(error.Message, "Error 1", MessageBoxButton.OK, MessageBoxImage.Exclamation);
						//MessageBox.Show("There is been an error connection to the server, please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					}
					return new();
				}
			}
		}
		public List<StructureTableViewClass> ColumnStructure {
			get {
				try
				{
					if (SelectedTable != null)
						return new(DomeinController.GetColumnsFromTable(_selectedTable).Select(column => new StructureTableViewClass(column.Name, column.__Type, column.IsNull, column.Extra, column)).ToList());
				} catch (Exception err)
				{
					MessageBox.Show(err.Message, "Error 2", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				return new();
			}
		}

		public BreadCrumb NavigationBreadCrumb => new BreadCrumb(server: _selectedServer, database: _selectedDatabase, table: _selectedTable);

		private Server _selectedServer;
		public Server SelectedServer {
			get {
				return _selectedServer;
			}
			set {
				if (_selectedServer == value)
					return;

				_selectedServer = value;
				ShowLoadingBar = true;
				IsServerSelected = true;

				Debug.WriteLine("Started OpenConnectionToServer()");
				Stopwatch stopwatch = new();
				stopwatch.Start();

				if (DomeinController.IsServerConnected)
					DomeinController.CloseConnectionToServer();

				DomeinController.OpenConnectionToServer(value);

				Debug.WriteLine(stopwatch.Elapsed.TotalSeconds + " seconds");
				Debug.WriteLine("Ended OpenConnectionToServer()");

				OnPropertyChanged(nameof(SelectedServer));
				OnPropertyChanged(nameof(DatabaseList));
				OnPropertyChanged(nameof(StructuurTypes));
				OnPropertyChanged(nameof(NavigationBreadCrumb));

				ShowLoadingBar = false;
			}
		}

		private bool _primaryKeyChecked = false;
		public bool PrimaryKeyChecked {
			get {
				return _primaryKeyChecked;
			}
			set {
				if (_primaryKeyChecked == value)
					return;
				_primaryKeyChecked = value;
				OnPropertyChanged(nameof(PrimaryKeyChecked));
			}
		}

		private bool _nullChecked = false;
		public bool NullChecked {
			get {
				return _nullChecked;
			}
			set {
				if (_nullChecked == value)
					return;
				_nullChecked = value;
				OnPropertyChanged(nameof(PrimaryKeyChecked));
			}
		}

		private bool _isServerSelected = false;
		public bool IsServerSelected {
			get {
				return _isServerSelected;
			}
			set {
				if (_isServerSelected == value)
					return;
				_isServerSelected = value;
				OnPropertyChanged(nameof(IsServerSelected));
			}
		}

		public bool ShowLoadingBar {
			get => DomeinController.ShowLoadingBar;
			set {
				if (ShowLoadingBar != value)
					DomeinController.ShowLoadingBar = value;
				OnPropertyChanged(nameof(ShowLoadingBar));
			}
		}

		public bool IsDataBaseSelected {
			get {
				return DomeinController.IsDatabaseConnected;
			}
		}

		private Visibility tablesPageVisibility = Visibility.Collapsed;
		public Visibility TablesPageVisibility {
			get {
				if (IsDataBaseSelected)
					return tablesPageVisibility;
				return Visibility.Collapsed;
			}
			set => tablesPageVisibility = value;
		}

		private Visibility structurePageVisibility = Visibility.Collapsed;
		public Visibility StructurePageVisibility {
			get {
				if (IsTableSelected)
					return structurePageVisibility;
				return Visibility.Collapsed;
			}

			set => structurePageVisibility = value;
		}

		private Visibility sqlQueryPageVisibility = Visibility.Collapsed;
		public Visibility SqlQueryPageVisibility {
			get {
				if (IsDataBaseSelected)
					return sqlQueryPageVisibility;
				return Visibility.Collapsed;
			}

			set => sqlQueryPageVisibility = value;
		}

		public bool AnyServersAvailable => DomeinController.GetServers().Count > 0;

		public bool IsServerConnected => DomeinController.IsServerConnected;

		private Database _selectedDatabase;
		public Database SelectedDatabase {
			get {
				return _selectedDatabase;
			}
			set {
				if (_selectedDatabase == value)
					return;

				//Async timeout that the user won't spam the database List.
				ShowLoadingBar = true;
				_selectedDatabase = value;

				DomeinController.UseDatabase(value);

				TablesPageVisibility = Visibility.Visible;
				StructurePageVisibility = Visibility.Collapsed;
				SqlQueryPageVisibility = Visibility.Collapsed;

				NotifyDatabaseSelectionChanged?.Invoke();

				OnPropertyChanged(nameof(TablesPageVisibility));
				OnPropertyChanged(nameof(StructurePageVisibility));
				OnPropertyChanged(nameof(SqlQueryPageVisibility));

				OnPropertyChanged(nameof(SelectedDatabase));
				OnPropertyChanged(nameof(Tables));
				OnPropertyChanged(nameof(IsDataBaseSelected));
				OnPropertyChanged(nameof(NavigationBreadCrumb));
				ShowLoadingBar = false;
			}
		}

		public bool IsTableSelected => SelectedTable != null;

		private static string _selectedTable;
		public string SelectedTable {
			get => _selectedTable;
			set {
				if (_selectedTable == value)
					return;
				_selectedTable = value;
				OnPropertyChanged(nameof(SelectedTable));
				OnPropertyChanged(nameof(ColumnStructure));
				OnPropertyChanged(nameof(IsTableSelected));
				OnPropertyChanged(nameof(NavigationBreadCrumb));
			}
		}

		public bool IsColumnSelected => SelectedColumn != null;

		private static StructureTableViewClass _selectedColumn;
		public StructureTableViewClass SelectedColumn {
			get => _selectedColumn;
			set {
				if (_selectedColumn == value)
					return;
				_selectedColumn = value;
				OnPropertyChanged(nameof(SelectedColumn));
				OnPropertyChanged(nameof(IsColumnSelected));
			}
		}

		#region Commands
		public ICommand AddTable {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					AddTableToDatabase(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}
		public ICommand DropTable {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					RemoveTableFromDatabase(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}

		public ICommand AddDatabase {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					if (Validate.NullOrWhiteSpace(NewDatabaseToAdd))
						return;
					DomeinController.AddDatabaseToServer(NewDatabaseToAdd);
					OnPropertyChanged(nameof(DatabaseList));
				});
			}
		}

		public ICommand RemoveDatabase {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					if (!IsDataBaseSelected || SelectedDatabase == null)
						return;
					DomeinController.RemoveDatabaseFromServer(SelectedDatabase);
					SelectedDatabase = null;
					TablesPageVisibility = Visibility.Visible;
					StructurePageVisibility = Visibility.Collapsed;
					SqlQueryPageVisibility = Visibility.Collapsed;

					OnPropertyChanged(nameof(TablesPageVisibility));
					OnPropertyChanged(nameof(StructurePageVisibility));
					OnPropertyChanged(nameof(SqlQueryPageVisibility));
					OnPropertyChanged(nameof(DatabaseList));
					OnPropertyChanged(nameof(IsDataBaseSelected));
					OnPropertyChanged(nameof(NavigationBreadCrumb));
				});
			}
		}

		public ICommand NewColumn {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					OpenNewColumnWindow(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}

		public ICommand RemoveColumnFromTable {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					if (!IsColumnSelected || SelectedColumn == null)
						return;
					DomeinController.RemoveColumnFromTable(SelectedColumn.Name, DomeinController.SelectedTable);
					OnPropertyChanged(nameof(ColumnStructure));
				});
			}
		}

		public ICommand ChangeTableStructure {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					ChangeTable(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}

		public ICommand SelectTablesPage {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					TablesPageVisibility = Visibility.Visible;
					StructurePageVisibility = Visibility.Collapsed;
					SqlQueryPageVisibility = Visibility.Collapsed;

					OnPropertyChanged(nameof(TablesPageVisibility));
					OnPropertyChanged(nameof(StructurePageVisibility));
					OnPropertyChanged(nameof(SqlQueryPageVisibility));
				});
			}
		}

		public ICommand SelectStructuurPage {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					TablesPageVisibility = Visibility.Collapsed;
					StructurePageVisibility = Visibility.Visible;
					SqlQueryPageVisibility = Visibility.Collapsed;

					OnPropertyChanged(nameof(TablesPageVisibility));
					OnPropertyChanged(nameof(StructurePageVisibility));
					OnPropertyChanged(nameof(SqlQueryPageVisibility));
				});
			}
		}

		public (List<string>, List<List<string>>) ExecuteCustomSqlQuery()
		{
			if (!Validate.NullOrWhiteSpace(CustomSqlQueryText))
			{
				Table table = DomeinController.SqlQuery(CustomSqlQueryText.Trim());

				List<string> headerColumns = table.Columns.Select(column => column.Name).ToList();
				List<List<string>> resultRows = table.Rows.Select(item => item.Items).ToList();

				return (headerColumns, resultRows);
			}
			return new();
		}

		public ICommand SelectSqlQueryPage {
			get {
				return new RelayCommand<object>((_none_) =>
				{
					TablesPageVisibility = Visibility.Collapsed;
					StructurePageVisibility = Visibility.Collapsed;
					SqlQueryPageVisibility = Visibility.Visible;

					OnPropertyChanged(nameof(TablesPageVisibility));
					OnPropertyChanged(nameof(StructurePageVisibility));
					OnPropertyChanged(nameof(SqlQueryPageVisibility));
				});
			}
		}

		#endregion

		#region Command Methods

		private static void AddTableToDatabase(object obj)
		{
			try
			{
				string table = NewTableToAdd.ToString();
				if (!Validate.NullOrWhiteSpace(table))
					DomeinController.WriteTableToDatabase(table);
			} catch (Exception err)
			{
				MessageBox.Show(err.Message, "Error 3", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private static void ChangeTable(object obj)
		{
			try
			{
				if (_selectedColumn == null)
					return;
				ColumnWindow ColumnWindow = new(_selectedColumn);
				ColumnWindow.Show();
			} catch (Exception err)
			{
				MessageBox.Show(err.Message, "Error 4", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private static void RemoveTableFromDatabase(object obj)
		{
			if (_selectedTable != null)
			{
				try
				{
					DomeinController.RemoveTableFromDatabase(_selectedTable);
				} catch (Exception err)
				{
					MessageBox.Show(err.Message, "Error 5", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
		}

		private static void OpenNewColumnWindow(object obj)
		{
			ColumnWindow ColumnWindow = new();
			ColumnWindow.Show();
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			try
			{
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			} catch (Exception error)
			{
				MessageBox.Show(error.Message, "Error 6", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
	}
}
