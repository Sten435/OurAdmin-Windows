using Domein.Controllers;
using Domein.DataBase;
using Domein.DataBase.DataTable;
using GUI.Views.ViewClasses;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GUI.ViewModels
{
	public class ViewModelBase : INotifyPropertyChanged
	{

		public ObservableCollection<Server> ServerList => new(DomeinController.GetServers().OrderBy(server => server.Host.Length));
		public ObservableCollection<Database> DatabaseList => new(DomeinController.GetDatabasesFromServer().OrderBy(database => database.Name.Length));

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
				OnPropertyChanged(nameof(SelectedServer));
				OnPropertyChanged(nameof(NavigationBreadCrumb));
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

					OnPropertyChanged(nameof(SelectedDatabase));
					OnPropertyChanged(nameof(NavigationBreadCrumb));
					CanSelectDatabase = true;
				});
			}
		}

		private Table _selectedTable;
		public Table SelectedTable {
			get {
				return _selectedTable;
			}
			set {
				if (_selectedTable == value)
					return;
				_selectedTable = value;
				OnPropertyChanged(nameof(SelectedTable));
				OnPropertyChanged(nameof(NavigationBreadCrumb));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
