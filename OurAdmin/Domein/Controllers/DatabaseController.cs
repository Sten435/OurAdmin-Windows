using Domein.DataBase;
using Domein.DataBase.Exceptions;
using Domein.DataBase.Sql;
using ReposInterface;
using System.Collections.Generic;
using System.Linq;

namespace Domein.Controllers {

	public class DatabaseController {
		private readonly IServerInfo _databasesRepo;

		private Server _connectedServer = null;
		private bool isServerConnected;

		public Database ConnectedDatabase { get; set; }

		public DatabaseController(IServerInfo databasesRepo) {
			_databasesRepo = databasesRepo;
		}

		public QueryResult SqlQuery(string query) => _databasesRepo.SqlQuery(query);

		//Server
		public void AddServer(Server server) => _databasesRepo.AddServer(server);

		public void RemoveServer(Server server) {
			if (_connectedServer != null || isServerConnected) throw new DatabaseException("Server has an open connection, close it first");
			_databasesRepo.RemoveServer(server);
		}

		public HashSet<Server> GetServers() => _databasesRepo.GetServers();

		public void OpenConnectionToServer(Server server) {
			if (_connectedServer != null && isServerConnected) throw new DatabaseException("There is an existing connected database, close it and try again");
			List<Server> servers = _databasesRepo.GetServers().ToList();
			if (!servers.Contains(server)) throw new DatabaseException($"The server: {server.Host} does not exist, add it first");
			_connectedServer = server;
			isServerConnected = true;
		}

		public void CloseConnectionToServer() {
			if (_connectedServer == null || !isServerConnected) throw new DatabaseException("There is no database currently connected");
			ConnectedDatabase = null;
			_connectedServer = null;
			isServerConnected = false;
		}

		//Database
		public void UseDatabase(Database database) {
			if (_connectedServer == null || !isServerConnected) throw new DatabaseException("There is no server currently connected");
			if (!_connectedServer.Databases.Contains(database)) throw new DatabaseException($"There is no such database available on server: {_connectedServer.Host}");
			_databasesRepo.UseDatabase(database);
			ConnectedDatabase = database;
		}

		public void AddDatabase(Database database) {
			if (_connectedServer == null || !isServerConnected) throw new DatabaseException("No server connected, please connect to a server");
			if (_connectedServer.Databases.Contains(database)) throw new DatabaseException("Database already exists in the server");
			_databasesRepo.AddDatabase(_connectedServer, database);
		}

		public void RemoveDatabase(Database database) {
			if (_connectedServer == null || !isServerConnected) throw new DatabaseException("No server connected, please connect to a server");
			if (ConnectedDatabase == null) throw new DatabaseException("No database connected");
			if (ConnectedDatabase.Equals(database)) ConnectedDatabase = null;
			_databasesRepo.RemoveDatabase(_connectedServer, database);
		}
	}
}