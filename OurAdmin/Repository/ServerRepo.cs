using Domein.DataBase;
using MySqlConnector;
using ReposInterface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace Repository {

	public class ServerRepo : IServerInfo {
		private readonly HashSet<Server> _serverList;
		private Database _selectedDatabase { get; set; }

		public ServerRepo() {
			_serverList = new HashSet<Server>();
		}

		public void AddDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Add(database);

		public void AddServer(Server database) => _serverList.Add(database);

		public HashSet<Server> GetServers() => _serverList;

		public void RemoveDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Remove(database);

		public void RemoveServer(Server database) => _serverList.Remove(database);

		public DataTable SqlQuery(Server server, string query) {
			using var connection = new MySqlConnection(server.ConnectionString);
			connection.Open();

			if (_selectedDatabase != null && connection.Database != _selectedDatabase.Name)
				connection.ChangeDatabase(_selectedDatabase.Name);

			using var command = new MySqlCommand(query.Replace(";", ""), connection);
			using DataTable dataTable = new();
			using MySqlDataAdapter MysqlDataAdapter = new(command);

			MysqlDataAdapter.Fill(dataTable);

			return dataTable;
		}

		public void UseDatabase(Server server, Database database) {
			_selectedDatabase = database;
		}
	}
}