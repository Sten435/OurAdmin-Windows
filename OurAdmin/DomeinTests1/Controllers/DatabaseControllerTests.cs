﻿using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Domein.DataBase.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReposInterface;
using Repository;
using System.Diagnostics;

namespace Domein.Controllers.Tests {

	[TestClass()]
	public class DatabaseControllerTests {
		private IServerInfo ServerRepo;
		private DatabaseController databaseController;
		private DomeinController domeinController;

		private Server server1;
		private Server server2;
		private Server server3;

		private UserInfo user2;
		private UserInfo user3;

		private Database database1;
		private Database database2;
		private Database database3;

		private List<Table> tables;

		[TestInitialize()]
		public void Initialize() {
			ServerRepo = new ServerRepo();
			databaseController = new(ServerRepo);
			domeinController = new(databaseController);

			tables = new();

			database1 = new("testDatabase1", tables);
			database2 = new("testDatabase2", tables);
			database3 = new("testDatabase3", tables);

			user2 = new("stan", "pwd123");
			user3 = new("user", "banz");

			server1 = null;
			server2 = new("localhost", user2, new() { database2 });
			server3 = new("combel", user3, new());
		}

		[TestMethod()]
		public void OpenConnectionToServer() {
			//Check that you can't connect to a database if an other database is already connected.
			domeinController.AddServer(server2);
			domeinController.OpenConnectionToServer(server2);
			domeinController.CloseConnectionToServer();
			domeinController.OpenConnectionToServer(server2);
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.OpenConnectionToServer(server2);
			});
			domeinController.CloseConnectionToServer();
		}

		[TestMethod()]
		public void CloseConnectionToServer() {
			//Check that you can't close a database if there is none connected.
			domeinController.AddServer(server2);
			domeinController.OpenConnectionToServer(server2);
			domeinController.CloseConnectionToServer();
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.CloseConnectionToServer();
			});
			domeinController.OpenConnectionToServer(server2);
			domeinController.CloseConnectionToServer();
		}

		[TestMethod()]
		public void AddServerTest() {
			//Check that you can't add servers that are not valid
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.AddServer(server1);
			});
			domeinController.AddServer(server2);
		}

		[TestMethod()]
		public void RemoveServerTest() {
			//Check that you can't remove a server that does not exist.
			domeinController.AddServer(server2);
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.RemoveServer(server1);
			});

			domeinController.RemoveServer(server2);
			domeinController.AddServer(server2);
			domeinController.OpenConnectionToServer(server2);

			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.RemoveServer(server2);
			});

			domeinController.CloseConnectionToServer();
			domeinController.RemoveServer(server2);
		}

		[TestMethod()]
		public void GetServersTest() {
			//Check that you get the inserted servers back.
			domeinController.AddServer(server2);
			domeinController.AddServer(server2);
			domeinController.AddServer(server3);

			int serverCount = domeinController.GetServers().Count();
			Debug.WriteLine($"Amount of servers: {serverCount}");

			if (serverCount != 2) Assert.Fail();
		}

		[TestMethod()]
		public void SqlQueryTest() {
			//Check if the query gives the correct reponse.
			QueryResult result = domeinController.SqlQuery("Testing Query");
			if (result.Data.ToString()?.Trim() == "") Assert.Fail();
		}

		[TestMethod()]
		public void UseDatabaseTest() {
			//Check that you can't use a non added database from the connected server.
			domeinController.AddServer(server2);
			domeinController.OpenConnectionToServer(server2);

			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.UseDatabase(database1);
			});

			domeinController.UseDatabase(database2);
			if (domeinController.ConnectedDatabase.Name != database2.Name) Assert.Fail();
		}

		[TestMethod()]
		public void AddDatabaseTest() {
			//Check that you can't add the same database to a server.
			domeinController.AddServer(server3);
			domeinController.OpenConnectionToServer(server3);
			domeinController.AddDatabase(database3);
			domeinController.AddDatabase(database2);
			domeinController.UseDatabase(database3);

			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.AddDatabase(database3);
			});

			domeinController.UseDatabase(database3);
		}

		[TestMethod()]
		public void RemoveDatabaseTest() {
			//Check you can't remove the same database you already removed.
			domeinController.AddServer(server3);
			domeinController.OpenConnectionToServer(server3);
			domeinController.AddDatabase(database2);
			domeinController.AddDatabase(database3);
			domeinController.UseDatabase(database3);
			domeinController.RemoveDatabase(database3);

			Assert.IsNull(domeinController.ConnectedDatabase);
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.RemoveDatabase(database3);
			});
		}
	}
}