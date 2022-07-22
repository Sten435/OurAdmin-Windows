using Domein.DB;
using Domein.DBConnectionInfo;
using Domein.Exceptions;
using ReposInterface;
using System;
using System.Collections.Generic;

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
		public void RemoveServer(Server server) => _databasesRepo.RemoveServer(server);

		public HashSet<Server> GetServers() => _databasesRepo.GetServers();
		public void OpenConnectionToServer(Server server) {
			if (isServerConnected) throw new DatabaseException("There is an existing connected database, close it and try again");
			_connectedServer = server;
			isServerConnected = true;
		}
		public void CloseConnectionToServer() {
			if (!isServerConnected) throw new DatabaseException("There is no database currently connected");
			ConnectedDatabase = null;
			isServerConnected = false;
		}

		//Database
		public void UseDatabase(Database database) {
			if (!isServerConnected) throw new DatabaseException("There is no server currently connected");
			_databasesRepo.UseDatabase(_connectedServer, database);
			ConnectedDatabase = database;
		}
		public void AddDatabase(Database database) {
			if (_connectedServer == null || !isServerConnected) throw new DatabaseException("No server connected, please connect to a server");
			_databasesRepo.AddDatabase(_connectedServer, database);
		}
		public void RemoveDatabase(Database database) {
			if (_connectedServer == null || !isServerConnected) throw new DatabaseException("No server connected, please connect to a server");
			_databasesRepo.RemoveDatabase(_connectedServer, database);
		}
	}

}
