using Domein.DBConnectionInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReposInterface {
	public interface IDatabaseConInfo {
		public IEnumerable<DatabaseConnectionInfo> GetDatabasesInfo();
		public void AddDatabase(DatabaseConnectionInfo database);
		public void RemoveDatabase(DatabaseConnectionInfo database);
	}
}
