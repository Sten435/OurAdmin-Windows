using Domein.DB;
using Domein.DBConnectionInfo;
using System;
using System.Collections.Generic;

namespace Domein.Controllers {
	public class DomeinController {
		public readonly DatabaseController DatabaseController;
		public string ConnectedDatabaseName => DatabaseController.ConnectedDatabase.Name;
		public IReadOnlyList<Table> ConnectedDatabaseTables => DatabaseController.ConnectedDatabase.Tables;

		public QueryResult SqlQuery(string query) => DatabaseController.ConnectedDatabase.SqlQuery(query);
		public Table ResultToTable(QueryResult queryResult) {
			throw new NotImplementedException("Not implemented");
		}
		public void Connect(Database database) => DatabaseController.Connect(database);
		public void Close() => DatabaseController.Close();
		public void AddDatabase(DatabaseConnectionInfo database) => DatabaseController.AddDatabase(database);
		public void RemoveDatabase(DatabaseConnectionInfo database) => DatabaseController.RemoveDatabase(database);

		public DomeinController(DatabaseController databaseController) {
			this.DatabaseController = databaseController;
		}
	}

}
