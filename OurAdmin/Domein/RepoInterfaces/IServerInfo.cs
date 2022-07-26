using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Sql;
using System.Collections.Generic;
using System.Data;

namespace ReposInterface {

	public interface IServerInfo {

		public HashSet<Server> GetServers();

		public void AddServer(Server server);

		public void RemoveServer(Server server);

		public void AddDatabase(Server server, Database database);

		public void RemoveDatabase(Server server, Database database);

		public void UseDatabase(Server server, Database database);

		DataTable SqlQuery(Server server, string query);
	}
}