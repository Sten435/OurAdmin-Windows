using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.Validatie;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Domein.Controllers
{

	public class DomeinController
	{
		public readonly DatabaseController DatabaseController;
		/// <summary>
		/// Get the connected database.
		/// </summary>
		public Database ConnectedDatabase => DatabaseController.ConnectedDatabase;
		/// <summary>
		/// See if there is any database connected.
		/// </summary>
		public bool IsDatabaseConnected => DatabaseController.IsDatabaseConnected;

		public DomeinController(DatabaseController databaseController)
		{
			this.DatabaseController = databaseController;
		}

		/// <summary>
		/// Export any object to json.
		/// </summary>
		/// <param name="obj">The object that needs to be converted.</param>
		/// <returns>The string representation of the given object.</returns>
		public static string ToJson(object obj)
		{
			var jsonConvertSettings = new JsonSerializerSettings
			{
				MaxDepth = null
			};
			return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonConvertSettings);
		}

		/// <summary>
		/// Passes a given query to the databaseController.
		/// </summary>
		/// <param name="query"></param>
		/// <returns>The result of the query in table format.</returns>
		public Table SqlQuery(string query) => DatabaseController.SqlQuery(query);

		/// <summary>
		/// Get the list of servers from the databaseController.
		/// </summary>
		/// <returns>All the available servers from the serverList.</returns>
		public List<Server> GetServers() => DatabaseController.GetServers().ToList();

		/// <summary>
		/// Get a List of all the databases from the connected server.
		/// </summary>
		/// <returns>List of databases.</returns>
		public List<Database> GetConnectedServerDatabases() => DatabaseController.GetConnectedServerDatabases().ToList();

		/// <summary>
		/// Close the current connected server.
		/// </summary>
		public void CloseConnectionToServer() => DatabaseController.CloseConnectionToServer();

		/// <summary>
		/// Open connection to a passed server.
		/// </summary>
		/// <param name="server">The server where you want to connect to.</param>
		public void OpenConnectionToServer(Server server) => Validate.ValidateObject(parameter: server, callBack: DatabaseController.OpenConnectionToServer, errorMessage: "Server can't be empty");

		/// <summary>
		/// Add a server to the serverList.
		/// </summary>
		/// <param name="server">The server object that you want to add to the serverList.</param>
		public void AddServer(Server server) => Validate.ValidateObject(parameter: server, callBack: DatabaseController.AddServer, errorMessage: "Server can't be empty");

		/// <summary>
		/// Removes a server from the serverList.
		/// </summary>
		/// <param name="server">The server that needs to be removed from the serverList.</param>
		/// <exception cref="DatabaseException"></exception>
		public void RemoveServer(Server server) => Validate.ValidateObject(parameter: server, callBack: DatabaseController.RemoveServer, errorMessage: "Server can't be empty");

		/// <summary>
		/// You can add databases to the connected server.
		/// </summary>
		/// <param name="database">The database to add.</param>
		/// <exception cref="DatabaseException"></exception>
		public void UseDatabase(Database database) => Validate.ValidateObject(parameter: database, callBack: DatabaseController.UseDatabase, errorMessage: "Database can't be empty");

		/// <summary>
		/// You can add databases to the connected server.
		/// </summary>
		/// <param name="database">The database to add.</param>
		/// <exception cref="DatabaseException"></exception>
		public void AddDatabase(Database database) => Validate.ValidateObject(parameter: database, callBack: DatabaseController.AddDatabase, errorMessage: "Database can't be empty");

		/// <summary>
		/// Remove a database from the connected server.
		/// </summary>
		/// <param name="database">The database to remove.</param>
		/// <exception cref="DatabaseException"></exception>
		public void RemoveDatabase(Database database) => Validate.ValidateObject(parameter: database, callBack: DatabaseController.RemoveDatabase, errorMessage: "Database can't be empty");

		/// <summary>
		/// Get the list of server from the databaseController.
		/// </summary>
		/// <returns>All the available servers from the serverList.</returns>
		public List<Database> GetDatabasesFromServer() => SqlQuery("show databases;").Rows.Select(row => row.Items).Select(dbName => new Database(dbName.First().ToString())).ToList();
	}
}