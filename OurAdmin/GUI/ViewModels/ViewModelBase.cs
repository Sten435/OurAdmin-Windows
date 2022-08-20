using Domein.Controllers;
using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Domein.Validatie;
using GUI.Views;
using GUI.Views.ViewClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		public static string NewTableToAdd { get; set; } = string.Empty;
		public static string NewColumnName { get; set; } = string.Empty;

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
				} catch (Exception)
				{
					if (IsServerConnected)
					{
						DomeinController.CloseConnectionToServer();
						DomeinController.RemoveServer(DomeinController.GetServers().Find(server => server.Host == SelectedServer.Host));
						OnPropertyChanged(nameof(ServerList));
						MessageBox.Show("There is been an error connection to the server, please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
						return new(DomeinController.GetColumnsFromTable(_selectedTable).Select(column => new StructureTableViewClass(column.Name, column.__Type, column.IsNull, column.Extra, column)));
					return new();
				} catch (Exception err)
				{
					MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return new();
				}
			}
		}

		public BreadCrumb NavigationBreadCrumb => new BreadCrumb(server: _selectedServer, database: _selectedDatabase);

		private Server _selectedServer;
		public Server SelectedServer {
			get {
				return _selectedServer;
			}
			set {
				if (_selectedServer == value)
					return;
				_selectedServer = value;
				IsServerSelected = true;
				DomeinController.OpenConnectionToServer(value);
				OnPropertyChanged(nameof(SelectedServer));
				OnPropertyChanged(nameof(DatabaseList));
				OnPropertyChanged(nameof(StructuurTypes));
				OnPropertyChanged(nameof(NavigationBreadCrumb));
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

		private bool _canSelectDatabase = true;
		public bool CanSelectDatabase {
			get {
				return _canSelectDatabase;
			}
			set {
				_canSelectDatabase = value;
				OnPropertyChanged(nameof(CanSelectDatabase));
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

		public bool IsDataBaseSelected {
			get {
				return DomeinController.IsDatabaseConnected;
			}
		}

		public bool AnyServersAvailable => DomeinController.GetServers().Count > 0;

		public bool IsServerConnected => DomeinController.IsServerConnected;

		private int selectedTabItemIndex = 0;
		public int SelectedTabItemIndex {
			get => selectedTabItemIndex;
			set {
				selectedTabItemIndex = value;
				OnPropertyChanged(nameof(SelectedTabItemIndex));
			}
		}

		private Database _selectedDatabase;
		public Database SelectedDatabase {
			get {
				return _selectedDatabase;
			}
			set {
				if (_selectedDatabase == value || !CanSelectDatabase)
					return;

				//Async timeout that the user won't spam the database List.
				Task.Run(() =>
				{
					CanSelectDatabase = false;
					Thread.Sleep(20);
					_selectedDatabase = value;
					DomeinController.UseDatabase(value);
					SelectedTabItemIndex = 0;

					OnPropertyChanged(nameof(SelectedDatabase));
					OnPropertyChanged(nameof(NavigationBreadCrumb));
					OnPropertyChanged(nameof(Tables));
					OnPropertyChanged(nameof(IsDataBaseSelected));
					CanSelectDatabase = true;
				});
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
				return new RelayCommand((_none_) =>
				{
					AddTableToDatabase(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}
		public ICommand DropTable {
			get {
				return new RelayCommand((_none_) =>
				{
					RemoveTableFromDatabase(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}
		public ICommand NewColumn {
			get {
				return new RelayCommand((_none_) =>
				{
					OpenNewColumnWindow(_none_);
					OnPropertyChanged(nameof(Tables));
				});
			}
		}

		public ICommand RemoveColumnFromTable {
			get {
				return new RelayCommand((_none_) =>
				{
					if (SelectedColumn == null)
						return;
					DomeinController.RemoveColumnFromTable(SelectedColumn.Name, DomeinController.SelectedTable);
					OnPropertyChanged(nameof(ColumnStructure));
				});
			}
		}

		public ICommand ChangeTableStructure {
			get {
				return new RelayCommand((_none_) =>
				{
					ChangeTable(_none_);
					OnPropertyChanged(nameof(Tables));
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
				MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private static void ChangeTable(object obj)
		{
			try
			{
				if (_selectedColumn == null)
					return;
				NewColumnWindow newColumnWindow = new(_selectedColumn);
				newColumnWindow.Show();
			} catch (Exception err)
			{
				MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
					MessageBox.Show(err.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
			}
		}

		private static void OpenNewColumnWindow(object obj)
		{
			NewColumnWindow newColumnWindow = new();
			newColumnWindow.Show();
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
				MessageBox.Show(error.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}
	}
}
