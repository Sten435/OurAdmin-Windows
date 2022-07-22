using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domein.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReposInterface;
using Repository;
using Domein.DB;
using Domein.Exceptions;
using Domein.DBConnectionInfo;
using System.Diagnostics;

namespace Domein.Controllers.Tests {
	[TestClass()]
	public class DatabaseControllerTests {
		private IServerInfo ServerRepo;
		private DatabaseController databaseController;
		private DomeinController domeinController;

		private Server server1;
		private Server server2;

		private UserInfo user1;
		private UserInfo user2;

		private Database database1;
		private Database database2;

		private List<Table> tables;

		[TestInitialize()]
		public void Initialize() {
			ServerRepo = new ServerRepo();
			databaseController = new(ServerRepo);
			domeinController = new(databaseController);

			tables = new();

			database1 = new("testDatabase1", tables);
			database2 = new("testDatabase2", tables);

			user1 = new("root", "");
			user2 = new("stan", "pwd123");

			server1 = null;
			server2 = new("combel", user2, new() { database2 });
		}

		[TestMethod()]
		public void OpenConnectionToServer() {
			//Check that you can't connect to a database if an other database is already connected.
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.OpenConnectionToServer(server2);
				domeinController.CloseConnectionToServer();
				domeinController.OpenConnectionToServer(server2);
				domeinController.OpenConnectionToServer(server2);
			});
		}

		[TestMethod()]
		public void CloseConnectionToServer() {
			//Check that you can't close a database if there is none connected.
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.OpenConnectionToServer(server2);
				domeinController.CloseConnectionToServer();
				domeinController.CloseConnectionToServer();
			});
		}

		[TestMethod()]
		public void AddServerTest() {
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.AddServer(server1);
			});
		}

		[TestMethod()]
		public void RemoveServerTest() {
			Assert.ThrowsException<DatabaseException>(() => {
				domeinController.RemoveServer(server1);
			});
		}

		[TestMethod()]
		public void GetServersTest() {
			domeinController.AddServer(server2);
			domeinController.AddServer(server2);

			int serverCount = domeinController.GetServers().Count();
			Debug.WriteLine($"Amount of servers: {serverCount}");

			if (serverCount != 1) Assert.Fail();
		}

		[TestMethod()]
		public void SqlQueryTest() {
			Assert.Fail();
		}

		[TestMethod()]
		public void UseDatabaseTest() {
			Assert.Fail();
		}

		[TestMethod()]
		public void AddDatabaseTest() {
			Assert.Fail();
		}

		[TestMethod()]
		public void RemoveDatabaseTest() {
			Assert.Fail();
		}
	}
}