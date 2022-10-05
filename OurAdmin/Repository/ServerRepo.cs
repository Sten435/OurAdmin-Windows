using Domein.Controllers;
using Domein.DataBase;
using Domein.DataBase.Exceptions;
using MySqlConnector;
using ReposInterface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace Repository
{

	public class ServerRepo : IServerInfo
	{
		private readonly HashSet<Server> _serverList;
		private Database _selectedDatabase {
			get; set;
		}
		public DatabaseType DatabaseType { get; set; }

		private MySqlConnection connection { get; set; }

		public ServerRepo(DatabaseType _databaseType)
		{
			this.DatabaseType = _databaseType;
			_serverList = new HashSet<Server>();
		}

		/// <summary>
		/// Add a database to the given server.
		/// </summary>
		/// <param name="server">The server where the database needs to be added.</param>
		/// <param name="database">The database that needs to be inserted into the server.</param>
		public void AddDatabase(Server server, Database database) => _serverList.First(svr => svr.Equals(server)).Databases.Add(database);

		/// <summary>
		/// Add a server to the serverList.
		/// </summary>
		/// <param name="server">The server that needs to be added.</param>
		public void AddServer(Server server) => _serverList.Add(server);

		public void OpenConnectionToServer(Server server)
		{
			if (connection != null && connection?.State != ConnectionState.Open)
				connection.Close();
			connection = new MySqlConnection(server.ConnectionString);
		}

		public bool CheckConnectionToServer(Server server)
		{
			try
			{
				OpenConnectionToServer(server);
				connection.Open();
				connection = null;
				return true;
			} catch (Exception)
			{
				return false;
			}
		}

		public void CloseConnectionToServer()
		{
			if (connection != null && connection?.State != ConnectionState.Open)
				connection.Close();
			_selectedDatabase = null;
			connection = null;
		}

		/// <summary>
		/// Get all the servers from the serverList.
		/// </summary>
		/// <returns>A HashSet with all the servers.</returns>
		public HashSet<Server> GetServers() => _serverList;

		/// <summary>
		/// Remove a database from a given server.
		/// </summary>
		/// <param name="server">The server where the database needs to be removed.</param>
		/// <param name="database">The database that needs to be removed.</param>
		public void RemoveDatabase(Server server, Database database)
		{
			if (_selectedDatabase.Name.ToLower() == database.Name.ToLower())
				_selectedDatabase = null;
			_serverList.First(svr => svr.Equals(server)).Databases.Remove(database);
		}

		/// <summary>
		/// Remove a server from the serverList.
		/// </summary>
		/// <param name="server">The server that needs to be removed.</param>
		public void RemoveServer(Server server) => _serverList.Remove(server);

		/// <summary>
		/// Execute the given query on the given server.
		/// </summary>
		/// <param name="server">The server where the query needs to be executed.</param>
		/// <param name="query">The query that needs to be executed.</param>
		/// <returns></returns>
		public DataTable SqlQuery(Server server, string query)
		{
			try
			{
				if (DatabaseType == DatabaseType.MYSQL)
				{
					Debug.WriteLine("Start - SqlQuery");
					Stopwatch stopwatch = new();
					stopwatch.Start();
					if (connection?.State != ConnectionState.Open)
					{
						OpenConnectionToServer(server);
						connection.Open();
					}
					Debug.WriteLine("Connection opend for: " + server.Host);
					if (_selectedDatabase != null && connection.Database != _selectedDatabase.Name)
						connection.ChangeDatabase(_selectedDatabase.Name);
					Debug.WriteLine("Started query: " + stopwatch.Elapsed.TotalSeconds);
					using var command = new MySqlCommand(query.Replace(";", string.Empty), connection);
					Debug.WriteLine("Ended query: " + stopwatch.Elapsed.TotalSeconds);
					using DataTable dataTable = new();
					Debug.WriteLine("Started query data: " + stopwatch.Elapsed.TotalSeconds);
					using MySqlDataAdapter MysqlDataAdapter = new(command);
					Debug.WriteLine("Ended query data: " + stopwatch.Elapsed.TotalSeconds);

					MysqlDataAdapter.Fill(dataTable);

					stopwatch.Stop();
					Debug.WriteLine(stopwatch.Elapsed.TotalSeconds + " seconds - SqlQuery");

					return dataTable;
				} else
					throw new NotImplementedException();
			} catch (Exception error)
			{
				if (error is MySqlException)
				{
					throw new DatabaseException($"There has been an error with the given query, please check the input fields.\n\n{error.Message}");
				}

				throw new Exception(error.Message);
			}
		}

		/// <summary>
		/// Correlates to 'use __database__' where the database is passed in, that will be executed on the given server.
		/// </summary>
		/// <param name="server">The server where the database is part of.</param>
		/// <param name="database">The database to use.</param>
		public void UseDatabase(Server server, Database database)
		{
			_selectedDatabase = database;
		}
	}
}