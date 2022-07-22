using Domein.DB;
using Domein.DBConnectionInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposInterface {
	public interface IServerInfo {
		public HashSet<Server> GetServers();
		public void AddServer(Server server);
		public void RemoveServer(Server server);
		public void AddDatabase(Server server, Database database);
		public void RemoveDatabase(Server server, Database database);
		public void UseDatabase(Server server, Database database);
		QueryResult SqlQuery(string query);
	}
}
