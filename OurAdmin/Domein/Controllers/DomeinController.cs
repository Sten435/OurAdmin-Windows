using DCD;
using Domein.Database;
using Domein.DatabaseConnectionInfo;
using System;

namespace Domein.Controllers {
	public class DomeinController {
		public QueryResult RunQuery(string query) {
			throw new NotImplementedException("Not implemented");
		}
		public Table ResultToTable(QueryResult queryResult) {
			throw new NotImplementedException("Not implemented");
		}
		public void Connect() {
			throw new NotImplementedException("Not implemented");
		}
		public void Close() {
			throw new NotImplementedException("Not implemented");
		}
		public void AddDatabase(UserInfo userCredentials, DatabaseConnectionInfo database) {
			throw new NotImplementedException("Not implemented");
		}
		public void RemoveDatabase(DatabaseConnectionInfo database) {
			throw new NotImplementedException("Not implemented");
		}

		private QueryController queryController;
		private DatabaseController[] databaseControllers;

	}

}
