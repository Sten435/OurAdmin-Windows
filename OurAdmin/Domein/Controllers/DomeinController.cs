using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.Validatie;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Domein.Controllers {

	public class DomeinController {
		public readonly DatabaseController DatabaseController;
		public Database ConnectedDatabase => DatabaseController.ConnectedDatabase;
		public bool IsDatabaseConnected => DatabaseController.IsDatabaseConnected;

		public DomeinController(DatabaseController databaseController) {
			this.DatabaseController = databaseController;
		}

		public static string ToJson<T>(T obj) {
			var jsonConvertSettings = new JsonSerializerSettings {
				MaxDepth = null
			};
			return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonConvertSettings);
		}

		public Table SqlQuery(string query) => DatabaseController.SqlQuery(query);

		public List<Server> GetServers() => DatabaseController.GetServers().ToList();

		public List<Database> GetConnectedServerDatabases() => DatabaseController.GetConnectedServerDatabases().ToList();

		public void CloseConnectionToServer() => DatabaseController.CloseConnectionToServer();

		public void OpenConnectionToServer(Server server) => Validate.ValidateDatabase(parameter: server, callBack: DatabaseController.OpenConnectionToServer, errorMessage: "Server can't be empty");

		public void AddServer(Server server) => Validate.ValidateDatabase(parameter: server, callBack: DatabaseController.AddServer, errorMessage: "Server can't be empty");

		public void RemoveServer(Server server) => Validate.ValidateDatabase(parameter: server, callBack: DatabaseController.RemoveServer, errorMessage: "Server can't be empty");

		public void UseDatabase(Database database) => Validate.ValidateDatabase(parameter: database, callBack: DatabaseController.UseDatabase, errorMessage: "Database can't be empty");

		public void AddDatabase(Database database) => Validate.ValidateDatabase(parameter: database, callBack: DatabaseController.AddDatabase, errorMessage: "Database can't be empty");

		public void RemoveDatabase(Database database) => Validate.ValidateDatabase(parameter: database, callBack: DatabaseController.RemoveDatabase, errorMessage: "Database can't be empty");
	}
}