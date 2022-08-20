using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Domein.DataBase.Table;
using Domein.Validatie;
using ReposInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Domein.Controllers
{

	public class DatabaseController
	{
		private readonly IServerInfo _databasesRepo;
		private Server _connectedServer = null;
		public DatabaseController(IServerInfo databasesRepo)
		{
			_databasesRepo = databasesRepo;
		}

		/// <summary>
		/// See if there is any server connected.
		/// </summary>
		public bool IsServerConnected;
		/// <summary>
		/// See if there is any database connected.
		/// </summary>
		public bool IsDatabaseConnected => _connectedServer != null && _connectedDatabase != null;

		public List<string> GetServerTypes()
		{
			return new() { "INT", "VARCHAR", "TEXT", "DATE", "TINYINT", "SMALLINT", "MEDIUMINT", "INT", "BIGINT", "DECIMAL", "FLOAT", "DOUBLE", "REAL", "BIT", "BOOLEAN", "SERIAL", "DATE", "DATETIME", "TIMESTAMP", "TIME", "YEAR", "CHAR", "VARCHAR", "TINYTEXT", "TEXT", "MEDIUMTEXT", "LONGTEXT", "BINARY", "VARBINARY", "TINYBLOB", "BLOB", "MEDIUMBLOB", "LONGBLOB", "ENUM", "SET", "GEOMETRY", "POINT", "LINESTRING", "POLYGON", "MULTIPOINT", "MULTILINESTRING", "MULTIPOLYGON", "GEOMETRYCOLLECTION", "JSON" };
		}

		private Database _connectedDatabase;

		/// <summary>
		/// Get the database that is in use.
		/// </summary>
		public Database ConnectedDatabase {
			get {
				if (IsDatabaseConnected)
					return _connectedDatabase;
				else
					throw new DatabaseException("There is no database currently connected");
			}
			set => _connectedDatabase = value;
		}

		/// <summary>
		/// Get columns from the selectedTable.
		/// </summary>
		/// <returns>List of columns where you can find data about them.</returns>
		public List<Column> GetColumnsFromTable()
		{
			return GetColumnStructure($"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE BINARY TABLE_NAME = '{DomeinController.SelectedTable}' AND TABLE_SCHEMA = '{ConnectedDatabase.Name}'").Columns.ToList();
		}

		public void WriteTableToDatabase(string cleanedTable)
		{
			try
			{
				string query = $"CREATE TABLE `{ConnectedDatabase}`.`{cleanedTable}` (`id` INT NOT NULL AUTO_INCREMENT , PRIMARY KEY (`id`))";
				SqlQuery(query);
			} catch (Exception err)
			{
				throw new DatabaseException(err.Message);
			}
		}

		public void RemoveTableFromDatabase(string cleanedTable)
		{
			try
			{
				string query = $"DROP TABLE `{ConnectedDatabase}`.`{cleanedTable}`";
				SqlQuery(query);
			} catch (Exception err)
			{
				throw new DatabaseException(err.Message);
			}
		}

		public void AddColumnToTable(Column newColumn, string selectedTable)
		{
			try
			{
				newColumn.__Type = newColumn.__Type.ToUpper();
				newColumn.__DefaultValue = newColumn.__DefaultValue.ToString().ToUpper();

				string typeLengthPart = (Validate.NullOrWhiteSpace(newColumn.__LengthValues) == false && (newColumn.__Type == "CHAR" || newColumn.__Type == "BINARY" || newColumn.__Type == "VARCHAR" || newColumn.__Type == "VARBINARY")) ? $"{newColumn.__Type}({newColumn.__LengthValues})" : $"{newColumn.__Type}";
				string attributesPart = newColumn.__Attributes == "NONE" ? string.Empty : $"{newColumn.__Attributes}";
				string __null = newColumn.IsNull == false ? $"NOT NULL" : $"NULL";
				string __default = Validate.NullOrWhiteSpace(newColumn.__DefaultValue.ToString()) == true ? string.Empty : $"{(newColumn.__DefaultValue.ToString() == "AS DEFINED" ? $"DEFAULT '{newColumn.__AsDefined}'" : newColumn.__DefaultValue.ToString().ToUpper() == "NONE" ? string.Empty : $"DEFAULT {newColumn.__DefaultValue}")}";
				string autoIncrement = newColumn.__AutoIncrement == false ? string.Empty : $"AUTO_INCREMENT";
				string comment = Validate.NullOrWhiteSpace(newColumn.__Comments) == true ? string.Empty : $"COMMENT '{newColumn.__Comments}'";
				string primaryKey = newColumn.__AutoIncrement == false ? string.Empty : $",ADD PRIMARY KEY (`{newColumn.Name.Trim()}`)";
				string query = $"ALTER TABLE `{selectedTable.Trim()}` ADD `{newColumn.Name.Trim()}` {typeLengthPart} {attributesPart} {__null} {__default} {autoIncrement} {comment} {primaryKey}";

				SqlQuery(query.Trim());
			} catch (Exception err)
			{
				throw new DatabaseException(err.Message);
			}
		}

		public void RemoveColumnFromTable(string columnName, string selectedTable)
		{
			try
			{
				string query = $"ALTER TABLE `{selectedTable.Trim()}` DROP COLUMN `{columnName.Trim()}`";

				SqlQuery(query);
			} catch (Exception err)
			{
				throw new DatabaseException(err.Message);
			}
		}

		public List<string> GetTables()
		{
			if (IsServerConnected && IsDatabaseConnected)
			{
				if (_databasesRepo.DatabaseType == DatabaseType.MYSQL)
				{
					List<string> res = SqlQuery("SHOW TABLES;").Rows.Select(row => row.Items[0].ToString()).ToList();
					return res;
				} else
				{
					//
				}

				throw new DatabaseException("There is no Database Type currently selected.");
			}

			if (!IsDatabaseConnected || !IsServerConnected)
				return new();
			if (!IsDatabaseConnected)
				throw new DatabaseException("There is currently no database connected");
			throw new DatabaseException("There is currently no server connected");
		}

		public List<string> GetServerAttributes()
		{
			return new() { "NONE", "BINARY", "UNSIGNED", "UNSIGNED ZEROFILL", "on update CURRENT_TIMESTAMP" };
		}

		public List<string> GetServerDefaults()
		{
			return new() { "NONE", "AS DEFINED", "NULL", "CURRENT_TIMESTAMP" };
		}


		/// <summary>
		/// Get databases from the connected server.
		/// </summary>
		/// <returns>A HashSet list with databases</returns>
		/// <exception cref="DatabaseException"></exception>
		public HashSet<Database> GetConnectedServerDatabases()
		{
			if (_connectedServer != null)
				return _connectedServer.Databases;
			else
				throw new DatabaseException("There is no server currently connected");
		}

		/// <summary>
		/// Run a custom crafted query.
		/// </summary>
		/// <param name="query">The query that needs to be executed.</param>
		/// <returns></returns>
		/// <exception cref="DatabaseException"></exception>
		public Table SqlQuery(string query)
		{
			if (_connectedServer != null)
			{
				DataTable dataTable = _databasesRepo.SqlQuery(_connectedServer, query);
				Table table = new();

				foreach (DataRow row in dataTable.Rows)
				{
					List<object> rowData = new();

					foreach (DataColumn col in dataTable.Columns)
					{
						Column column = new();

						table.Relations = col.Table.ChildRelations;

						column.Name = col.ColumnName;
						column.Extra = col.AutoIncrement == true ? "autoincrement" : string.Empty;
						column.IsNull = col.AllowDBNull;
						column.TypeAmount = col.MaxLength;
						column.__DefaultValue = col.DefaultValue;
						column.__Type = col.DataType.Name;
						column.SqlType = GetColumnType(query, column.Name);

						rowData.Add(row[col]);
						table.AddColumn(column);
					}

					table.AddRow(new Row(new(rowData)));
				}

				return table;
			} else
				throw new DatabaseException("There is no server currently connected");
		}

		/// <summary>
		/// Run a custom crafted query.
		/// </summary>
		/// <param name="query">The query that needs to be executed.</param>
		/// <returns></returns>
		/// <exception cref="DatabaseException"></exception>
		public Table GetColumnStructure(string query)
		{
			if (_connectedServer != null)
			{
				DataTable dataTable = _databasesRepo.SqlQuery(_connectedServer, query);
				Table table = new();
				DataRow dataRow = dataTable.Rows[0];
				string jsonString = DomeinController.ToJson(dataRow.Table);
				List<ColumnStructure> parsedJson = DomeinController.ParseJson<List<ColumnStructure>>(jsonString);

				foreach (ColumnStructure resultItem in parsedJson)
				{
					Column column = new();

					column.Name = resultItem.COLUMNNAME;
					column.IsNull = resultItem.ISNULLABLE.ToUpper() == "YES" ? true : false;
					column.Extra = resultItem.EXTRA.ToUpper().Contains("ON UPDATE CURRENT_TIMESTAMP") == false ? "" : "on update CURRENT_TIMESTAMP";
					column.__DefaultValue = resultItem.COLUMNDEFAULT;
					column.__AutoIncrement = resultItem.EXTRA == "auto_increment";
					column.__Comments = resultItem.COLUMNCOMMENT;
					column.__Attributes = column.Extra;
					column.__Type = resultItem.COLUMNTYPE;
					column.SqlType = resultItem.DATATYPE;
					table.AddColumn(column);
				}

				return table;
			} else
				throw new DatabaseException("There is no server currently connected");
		}


		/// <summary>
		/// Get the sqlType from a column, given is the givenQuery where the function extract the table from. This to get the sqlType for the column.
		/// </summary>
		/// <param name="givenQuery">The query to execute.</param>
		/// <param name="column">The column where you want the sqlType for.</param>
		/// <returns>The sqlType for the given column. (varchar, int, timestamp, ...)</returns>
		private string GetColumnType(string givenQuery, string column)
		{
			if (!IsDatabaseConnected)
				return "None";
			string table = string.Empty;

			DataTable dataTable = _databasesRepo.SqlQuery(_connectedServer, "show tables;");
			//Todo minder checken op tables nu elke keer ddat er een column gecheked moet worden

			List<string> tables = new();
			foreach (DataRow row in dataTable.Rows)
			{

				foreach (DataColumn col in dataTable.Columns)
				{
					tables.Add(row[col].ToString());
				}
			}

			for (int i = 0; i < tables.Count; i++)
			{
				string pattern = @$" +from{{1}} +(\b{Regex.Escape(tables[i])}\b){{1}}";

				Regex regex = new(pattern, RegexOptions.IgnoreCase);

				var isRegexFouned = regex.IsMatch(givenQuery);
				// There is a table found in the givenQuery that correcpondents to a table from the database;
				// This method of extracting the table from the givenQuery is not the safest one, but it does the job ;-)
				if (isRegexFouned)
				{
					table = tables[i];
					break;
				}
			}

			if (table != string.Empty)
			{
				dataTable = _databasesRepo.SqlQuery(_connectedServer, $"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE table_name='{table}' AND COLUMN_NAME='{column}';"); // Get the sql datatype from the column.

				if (dataTable.Rows.Count == 0)
					return "Try removing the 'as' operator (if presented)"; // This can indecate the use of 'as' in the query. as is used to rename a column in the result, Therefore we have no rows back becuse the made up column name does not exist in the table.

				foreach (DataRow row in dataTable.Rows)
				{
					foreach (DataColumn col in dataTable.Columns)
					{
						string sqlType = row[col].ToString();
						return sqlType;
					}
				}
			}
			return "None";
		}

		/// <summary>
		/// Add a server to the serverList.
		/// </summary>
		/// <param name="server">The server object that you want to add to the serverList.</param>
		public void AddServer(Server server) => _databasesRepo.AddServer(server);

		/// <summary>
		/// Removes a server from the serverList.
		/// </summary>
		/// <param name="server">The server that needs to be removed from the serverList.</param>
		/// <exception cref="DatabaseException"></exception>
		public void RemoveServer(Server server)
		{
			if (IsServerConnected && _connectedServer.Equals(server))
				throw new DatabaseException("Server has an open connection, close it first");
			_databasesRepo.RemoveServer(server);
		}

		/// <summary>
		/// Get all the servers from the serverList.
		/// </summary>
		/// <returns>A HashSet with all the servers from the serverList.</returns>
		public HashSet<Server> GetServers() => _databasesRepo.GetServers();

		/// <summary>
		/// Open connection to a server, this is required to use the server.
		/// </summary>
		/// <param name="server">The server that you want to connect to.</param>
		/// <exception cref="DatabaseException"></exception>
		public void OpenConnectionToServer(Server server)
		{
			if (_connectedServer != null && IsServerConnected)
				throw new DatabaseException("There is an existing connected database, close it and try again");
			List<Server> servers = _databasesRepo.GetServers().ToList();
			if (!servers.Contains(server))
				throw new DatabaseException($"The server: {server.Host} does not exist, add it first");
			_connectedServer = server;
			IsServerConnected = true;
		}

		/// <summary>
		/// Close the connection to the server.
		/// </summary>
		/// <exception cref="DatabaseException"></exception>
		public void CloseConnectionToServer()
		{
			if (_connectedServer == null || !IsServerConnected)
				throw new DatabaseException("There is no database currently connected");
			ConnectedDatabase = null;
			_connectedServer = null;
			IsServerConnected = false;
		}

		/// <summary>
		/// This corelates to the sql query 'use __database__' where the __database__ is passed in.
		/// </summary>
		/// <param name="database">The database where you want to connect to.</param>
		/// <exception cref="DatabaseException"></exception>
		public void UseDatabase(Database database)
		{
			if (_connectedServer == null || !IsServerConnected)
				throw new DatabaseException("There is no server currently connected");
			if (!_connectedServer.Databases.Contains(database))
				throw new DatabaseException($"There is no such database available on server: {_connectedServer.Host}");
			_databasesRepo.UseDatabase(_connectedServer, database);
			ConnectedDatabase = database;
		}

		/// <summary>
		/// You can add databases to the connected server.
		/// </summary>
		/// <param name="database">The database to add.</param>
		/// <exception cref="DatabaseException"></exception>
		public void AddDatabase(Database database)
		{
			if (_connectedServer == null || !IsServerConnected)
				throw new DatabaseException("No server connected, please connect to a server");
			if (_connectedServer.Databases.Contains(database))
				throw new DatabaseException("Database already exists in the server");
			_databasesRepo.AddDatabase(_connectedServer, database);
		}

		/// <summary>
		/// Remove a database from the connected server.
		/// </summary>
		/// <param name="database">The database to remove.</param>
		/// <exception cref="DatabaseException"></exception>
		public void RemoveDatabase(Database database)
		{
			if (_connectedServer == null || !IsServerConnected)
				throw new DatabaseException("No server connected, please connect to a server");
			if (ConnectedDatabase == null)
				throw new DatabaseException("No database connected");
			if (ConnectedDatabase.Equals(database))
				ConnectedDatabase = null;
			_databasesRepo.RemoveDatabase(_connectedServer, database);
		}
	}
}