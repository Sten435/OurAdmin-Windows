using Domein.DataBase;
using System.Collections.Generic;
using System.Data;

namespace ReposInterface
{

	public interface IServerInfo
	{
		public DatabaseType DatabaseType { get; set; }

		public HashSet<Server> GetServers();

		public void AddServer(Server server);

		public void RemoveServer(Server server);

		public void AddDatabase(Server server, Database database);

		public void RemoveDatabase(Server server, Database database);

		public void UseDatabase(Server server, Database database);

		public DataTable SqlQuery(Server server, string query);
	}
}