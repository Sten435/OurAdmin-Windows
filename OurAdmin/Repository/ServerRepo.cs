using Domein.DB;
using Domein.DBConnectionInfo;
using ReposInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository {
	public class ServerRepo : IServerInfo {
		private readonly HashSet<Server> _serverList;

		public ServerRepo() {
			_serverList = new HashSet<Server>();
		}

		public HashSet<Server> GetServers() => _serverList;
		public void AddServer(Server database) => _serverList.Add(database);
		public void RemoveServer(Server database) => _serverList.Remove(database);

		public void AddDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Add(database);
		public void RemoveDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Remove(database);

		public void UseDatabase(Server server, Database database) {
			throw new NotImplementedException();
		}
		public QueryResult SqlQuery(string query) {
			throw new NotImplementedException();
		}
	}
}
