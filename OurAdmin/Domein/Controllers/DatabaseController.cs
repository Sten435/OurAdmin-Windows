using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using ReposInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domein.Controllers {

	public class DatabaseController {
		private readonly IServerInfo _databasesRepo;

		private Server _connectedServer = null;
		public bool IsServerConnected;
		public bool IsDatabaseConnected => _connectedServer != null && _connectedDatabase != null;

		private Database _connectedDatabase;

		public Database ConnectedDatabase {
			get {
				if (IsDatabaseConnected)
					return _connectedDatabase;
				else throw new DatabaseException("There is no database currently connected");
			}
			set => _connectedDatabase = value;
		}

		public DatabaseController(IServerInfo databasesRepo) {
			_databasesRepo = databasesRepo;
		}

		public HashSet<Database> GetConnectedServerDatabases() {
			if (_connectedServer != null) return _connectedServer.Databases;
			else throw new DatabaseException("There is no server currently connected");
		}

		public Table SqlQuery(string query) {
			if (_connectedServer != null) {
				DataTable dataTable = _databasesRepo.SqlQuery(_connectedServer, query);
				Table table = new();

				foreach (DataRow row in dataTable.Rows) {
					List<object> rowData = new();

					foreach (DataColumn col in dataTable.Columns) {
						Column column = new();

						table.Relations = col.Table.ChildRelations;

						column.Name = col.ColumnName;
						column.AutoIncrement = col.AutoIncrement;
						column.IsNull = col.AllowDBNull;
						column.TypeAmount = col.MaxLength;
						column.DefaultValue = col.DefaultValue;
						column.Type = col.DataType.Name;
						column.SqlType = GetColumnType(query, column.Name);

						rowData.Add(row[col]);
						table.AddColumn(column);
					}

					table.AddRow(new Row(new(rowData)));
				}

				return table;
			} else throw new DatabaseException("There is no server currently connected");
		}

		private string GetColumnType(string givenQuery, string column) {
			if (!IsDatabaseConnected) return "None";
			string table = "";

			try {
				DataTable dataTable = _databasesRepo.SqlQuery(_connectedServer, "show tables;");
				//Todo minder checken op tables nu elke keer ddat er een column gecheked moet worden

				List<string> tables = new();
				foreach (DataRow row in dataTable.Rows) {

					foreach (DataColumn col in dataTable.Columns) {
						tables.Add(row[col].ToString());
					}
				}

				foreach (string tble in tables) {
					string pattern = @"\b" + Regex.Escape(tble) + @"\b";
					//Todo Regex ook checken of er FROM voor het woord staat

					Regex re = new Regex(pattern, RegexOptions.IgnoreCase);

					var filtered = re.IsMatch(givenQuery);
					if (filtered) {
						table = tble;
						break;
					};
				}

				if (table != "") {
					dataTable = _databasesRepo.SqlQuery(_connectedServer, $"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name='{table}' AND COLUMN_NAME='{column}';");
					foreach (DataRow row in dataTable.Rows) {
						foreach (DataColumn col in dataTable.Columns) {
							return row[col].ToString();
						}
					}
				}
			} catch { }
			return "None";
		}

		//Server
		public void AddServer(Server server) => _databasesRepo.AddServer(server);

		public void RemoveServer(Server server) {
			if (_connectedServer != null || IsServerConnected) throw new DatabaseException("Server has an open connection, close it first");
			_databasesRepo.RemoveServer(server);
		}

		public HashSet<Server> GetServers() => _databasesRepo.GetServers();

		public void OpenConnectionToServer(Server server) {
			if (_connectedServer != null && IsServerConnected) throw new DatabaseException("There is an existing connected database, close it and try again");
			List<Server> servers = _databasesRepo.GetServers().ToList();
			if (!servers.Contains(server)) throw new DatabaseException($"The server: {server.Host} does not exist, add it first");
			_connectedServer = server;
			IsServerConnected = true;
		}

		public void CloseConnectionToServer() {
			if (_connectedServer == null || !IsServerConnected) throw new DatabaseException("There is no database currently connected");
			ConnectedDatabase = null;
			_connectedServer = null;
			IsServerConnected = false;
		}

		//Database
		public void UseDatabase(Database database) {
			if (_connectedServer == null || !IsServerConnected) throw new DatabaseException("There is no server currently connected");
			if (!_connectedServer.Databases.Contains(database)) throw new DatabaseException($"There is no such database available on server: {_connectedServer.Host}");
			_databasesRepo.UseDatabase(_connectedServer, database);
			ConnectedDatabase = database;
		}

		public void AddDatabase(Database database) {
			if (_connectedServer == null || !IsServerConnected) throw new DatabaseException("No server connected, please connect to a server");
			if (_connectedServer.Databases.Contains(database)) throw new DatabaseException("Database already exists in the server");
			_databasesRepo.AddDatabase(_connectedServer, database);
		}

		public void RemoveDatabase(Database database) {
			if (_connectedServer == null || !IsServerConnected) throw new DatabaseException("No server connected, please connect to a server");
			if (ConnectedDatabase == null) throw new DatabaseException("No database connected");
			if (ConnectedDatabase.Equals(database)) ConnectedDatabase = null;
			_databasesRepo.RemoveDatabase(_connectedServer, database);
		}
	}
}