using Domein.DatabaseConnectionInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository {
	public class DatabaseConnectionInfoRepo : IDatabaseConInfo {
		readonly ICollection<DatabaseConnectionInfo> databaseListConnectionInfo;

		public IEnumerable<DatabaseConnectionInfo> GetDatabasesInfo() => databaseListConnectionInfo;
		public void AddDatabase(DatabaseConnectionInfo database) => databaseListConnectionInfo.Append(database);
		public void RemoveDatabase(DatabaseConnectionInfo database) => databaseListConnectionInfo.Remove(database);


	}
}
