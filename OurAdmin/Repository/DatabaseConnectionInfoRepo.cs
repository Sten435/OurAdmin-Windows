using Domein.DBConnectionInfo;
using ReposInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository {
	public class DatabaseConnectionInfoRepo : IDatabaseConInfo {
		private readonly ICollection<DatabaseConnectionInfo> _databaseListConnectionInfo;

		public IEnumerable<DatabaseConnectionInfo> GetDatabasesInfo() => _databaseListConnectionInfo;
		public void AddDatabase(DatabaseConnectionInfo database) => _databaseListConnectionInfo.Append(database);
		public void RemoveDatabase(DatabaseConnectionInfo database) => _databaseListConnectionInfo.Remove(database);

		public DatabaseConnectionInfoRepo() {
			_databaseListConnectionInfo = new HashSet<DatabaseConnectionInfo>();
		}
	}
}
