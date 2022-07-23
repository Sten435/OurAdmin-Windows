using Domein.DataBase;
using Domein.DataBase.Sql;
using ReposInterface;
using System.Collections.Generic;
using System.Linq;

namespace Repository {

	public class ServerRepo : IServerInfo {
		private readonly HashSet<Server> _serverList;

		public ServerRepo() {
			_serverList = new HashSet<Server>();
		}

		public void AddDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Add(database);

		public void AddServer(Server database) => _serverList.Add(database);

		public HashSet<Server> GetServers() => _serverList;

		public void RemoveDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Remove(database);

		public void RemoveServer(Server database) => _serverList.Remove(database);

		public QueryResult SqlQuery(string query) {
			return new QueryResult(query);
		}

		public void UseDatabase(Database database) {
			SqlQuery($"use {database.Name};");
		}
	}
}