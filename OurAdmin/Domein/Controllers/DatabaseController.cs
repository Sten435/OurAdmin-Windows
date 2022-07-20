using Domein.DB;
using Domein.DBConnectionInfo;
using Domein.Exceptions;
using ReposInterface;
using System.Collections.Generic;

namespace Domein.Controllers {
	public class DatabaseController : IDatabaseConInfo {
		private readonly IDatabaseConInfo _databasesRepo;

		private Database _connectedDatabase = null;
		private bool isConnected;

		public Database ConnectedDatabase {
			get {
				if (!isConnected) throw new DatabaseException("There is no database currently connected");
				return _connectedDatabase;
			}
			private set => _connectedDatabase = value;
		}

		public void Connect(Database database) {
			if (!isConnected) {
				ConnectedDatabase = database;
				//Close
				isConnected = true;
			} else throw new DatabaseException("There is an existing connected database, close it and try again");
		}
		public void Close() {
			if (isConnected) {
				//Close
				ConnectedDatabase = null;
				isConnected = false;
			} else throw new DatabaseException("There is no database currently connected");
		}
		public void AddDatabase(DatabaseConnectionInfo database) => _databasesRepo.AddDatabase(database);
		public void RemoveDatabase(DatabaseConnectionInfo database) => _databasesRepo.RemoveDatabase(database);
		public IEnumerable<DatabaseConnectionInfo> GetDatabasesInfo() => _databasesRepo.GetDatabasesInfo();

		public DatabaseController(IDatabaseConInfo databasesRepo) {
			_databasesRepo = databasesRepo;
		}
	}

}
