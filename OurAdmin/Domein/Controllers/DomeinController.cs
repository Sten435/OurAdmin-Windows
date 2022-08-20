using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Domein.DataBase.Table;
using Domein.Validatie;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domein.Controllers
{

	public sealed class DomeinController
	{
		public static DatabaseController DatabaseController;

		/// <summary>
		/// Get the connected database.
		/// </summary>
		public static Database ConnectedDatabase => DatabaseController.ConnectedDatabase;

		private static string selectedTable;

		public static void WriteTableToDatabase(string table)
		{
			string cleanedTable = table.Trim().Replace(" ", "_");

			DatabaseController.WriteTableToDatabase(cleanedTable);
		}

		public static List<string> GetServerTypes() => DatabaseController.GetServerTypes();

		public static void RemoveTableFromDatabase(string selectedTable)
		{
			string cleanedTable = selectedTable.Trim().Replace(" ", "_");

			DatabaseController.RemoveTableFromDatabase(cleanedTable);
		}

		/// <summary>
		/// Ge the selected table.
		/// </summary>
		public static string SelectedTable {
			get {
				if (selectedTable == string.Empty)
					throw new DatabaseException("No table is selected");
				else
					return selectedTable;
			}
			set {
				if (!string.IsNullOrWhiteSpace(value))
				{
					selectedTable = value;
				} else
					throw new DatabaseException("The new selected table is not valid");
			}
		}

		/// <summary>
		/// Get the structure of a table.
		/// </summary>
		/// <param name="selectedTable">Table to get the structure from.</param>
		/// <returns>List of Columns</returns>
		public static List<Column> GetColumnsFromTable(string selectedTable)
		{
			if (selectedTable == null)
				return new();
			SelectedTable = selectedTable;
			return DatabaseController.GetColumnsFromTable();
		}

		/// <summary>
		/// See if there is any database connected.
		/// </summary>
		public static bool IsDatabaseConnected => DatabaseController.IsDatabaseConnected;

		/// <summary>
		/// See if there is any server connected.
		/// </summary>
		public static bool IsServerConnected => DatabaseController.IsServerConnected;

		public static void AddColumnToTable(Column newColumn, string selectedTable)
		{
			DatabaseController.AddColumnToTable(newColumn, selectedTable);
		}

		public static void RemoveColumnFromTable(string columnName, string selectedTable)
		{
			DatabaseController.RemoveColumnFromTable(columnName, selectedTable);
		}

		/// <summary>
		/// Get all tables from the currently connected database.
		/// </summary>
		/// <returns>List of table names</returns>
		public static List<string> GetTables() => DatabaseController.GetTables();

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

		public static List<string> GetServerAttributes()
		{
			return DatabaseController.GetServerAttributes();
		}

		public static List<string> GetServerDefaults()
		{
			return DatabaseController.GetServerDefaults();
		}

		/// <summary>
		/// Get a dictionary from json.
		/// </summary>
		/// <param name="json">json</param>
		/// <returns>Returns a dictionary that was parsed from the given string.</returns>
		public static type ParseJson<type>(string json) => JsonConvert.DeserializeObject<type>(json);

		/// <summary>
		/// Passes a given query to the databaseController.
		/// </summary>
		/// <param name="query"></param>
		/// <returns>The result of the query in table format.</returns>
		public static Table SqlQuery(string query) => DatabaseController.SqlQuery(query);

		/// <summary>
		/// Get the list of servers from the databaseController.
		/// </summary>
		/// <returns>All the available servers from the serverList.</returns>
		public static List<Server> GetServers() => DatabaseController.GetServers().ToList();

		/// <summary>
		/// Get a List of all the databases from the connected server.
		/// </summary>
		/// <returns>List of databases.</returns>
		public static List<Database> GetConnectedServerDatabases() => DatabaseController.GetConnectedServerDatabases().ToList();

		/// <summary>
		/// Close the current connected server.
		/// </summary>
		public static void CloseConnectionToServer() => DatabaseController.CloseConnectionToServer();

		/// <summary>
		/// Open connection to a passed server.
		/// </summary>
		/// <param name="server">The server where you want to connect to.</param>
		public static void OpenConnectionToServer(Server server) => Validate.ValidateObject(parameter: server, callBack: DatabaseController.OpenConnectionToServer, errorMessage: "Server can't be empty");

		/// <summary>
		/// Add a server to the serverList.
		/// </summary>
		/// <param name="server">The server object that you want to add to the serverList.</param>
		public static void AddServer(Server server) => Validate.ValidateObject(parameter: server, callBack: DatabaseController.AddServer, errorMessage: "Server can't be empty");

		/// <summary>
		/// Removes a server from the serverList.
		/// </summary>
		/// <param name="server">The server that needs to be removed from the serverList.</param>
		/// <exception cref="DatabaseException"></exception>
		public static void RemoveServer(Server server) => Validate.ValidateObject(parameter: server, callBack: DatabaseController.RemoveServer, errorMessage: "Server can't be empty");

		/// <summary>
		/// You can add databases to the connected server.
		/// </summary>
		/// <param name="database">The database to add.</param>
		/// <exception cref="DatabaseException"></exception>
		public static void UseDatabase(Database database) => Validate.ValidateObject(parameter: database, callBack: DatabaseController.UseDatabase, errorMessage: "Database can't be empty");

		/// <summary>
		/// You can add databases to the connected server.
		/// </summary>
		/// <param name="database">The database to add.</param>
		/// <exception cref="DatabaseException"></exception>
		public static void AddDatabase(Database database) => Validate.ValidateObject(parameter: database, callBack: DatabaseController.AddDatabase, errorMessage: "Database can't be empty");

		/// <summary>
		/// Remove a database from the connected server.
		/// </summary>
		/// <param name="database">The database to remove.</param>
		/// <exception cref="DatabaseException"></exception>
		public static void RemoveDatabase(Database database) => Validate.ValidateObject(parameter: database, callBack: DatabaseController.RemoveDatabase, errorMessage: "Database can't be empty");


		private static HashSet<string> wontAddDatabases = new() { "information_schema", "mysql", "performance_schema", "sys", "phpmyadmin" };
		/// <summary>
		/// Get the list of server from the databaseController.
		/// </summary>
		/// <returns>All the available servers from the serverList.</returns>
		public static List<Database> GetDatabasesFromServer()
		{
			return SqlQuery("show databases;").Rows.Select(row => row.Items).Where(dbName => !wontAddDatabases.Contains(dbName.First())).Select(dbName => new Database(dbName.First().ToString())).ToList();
		}
	}
}