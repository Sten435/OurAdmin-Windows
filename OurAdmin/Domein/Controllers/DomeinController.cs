using Domein.DB;
using Domein.DBConnectionInfo;
using Domein.Validatie;
using System;
using System.Collections.Generic;

namespace Domein.Controllers {
	public class DomeinController {
		public readonly DatabaseController DatabaseController;
		public Database ConnectedDatabase => DatabaseController.ConnectedDatabase;

		public DomeinController(DatabaseController databaseController) {
			this.DatabaseController = databaseController;
		}

		public QueryResult SqlQuery(string query) => DatabaseController.SqlQuery(query);

		public HashSet<Server> GetServers() => DatabaseController.GetServers();
		public void CloseConnectionToServer() => DatabaseController.CloseConnectionToServer();
		public void OpenConnectionToServer(Server server) => Validate.ValidateDatabase(parameter: server, callBack: DatabaseController.OpenConnectionToServer, errorMessage: "Server can't be empty");
		public void AddServer(Server server) => Validate.ValidateDatabase(parameter: server, callBack: DatabaseController.AddServer, errorMessage: "Server can't be empty");
		public void RemoveServer(Server server) => Validate.ValidateDatabase(parameter: server, callBack: DatabaseController.RemoveServer, errorMessage: "Server can't be empty");

		public void UseDatabase(Database database) => Validate.ValidateDatabase(parameter: database, callBack: DatabaseController.UseDatabase, errorMessage: "Database can't be empty");
		public void AddDatabase(Database database) => Validate.ValidateDatabase(parameter: database, callBack: DatabaseController.AddDatabase, errorMessage: "Database can't be empty");
		public void RemoveDatabase(Database database) => Validate.ValidateDatabase(parameter: database, callBack: DatabaseController.RemoveDatabase, errorMessage: "Database can't be empty");
	}

}
