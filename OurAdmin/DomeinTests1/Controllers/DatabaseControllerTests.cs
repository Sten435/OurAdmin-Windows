using Domein.DataBase;
using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReposInterface;
using Repository;
using System.Diagnostics;

namespace Domein.Controllers.Tests
{

	[TestClass()]
	public class DatabaseControllerTests
	{
		private IServerInfo ServerRepo;
		private DatabaseController databaseController;

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
		public void Initialize()
		{
			ServerRepo = new ServerRepo(DatabaseType.MYSQL);
			databaseController = new(ServerRepo, DatabaseType.MYSQL);
			DomeinController.DatabaseController = databaseController;

			tables = new();

			database1 = new("testdatabase");
			database2 = new("testDatabase2");
			database3 = new("testDatabase3");

			user2 = new("root", string.Empty);
			user3 = new("user", "banz");

			server1 = null;
			server2 = new("localhost", user2);
			server3 = new("combel", user3);
		}

		[TestMethod()]
		public void OpenConnectionToServer()
		{
			//Check that you can't connect to a database if an other database is already connected.
			DomeinController.AddServer(server2);
			DomeinController.OpenConnectionToServer(server2);
			DomeinController.CloseConnectionToServer();
			DomeinController.OpenConnectionToServer(server2);
			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.OpenConnectionToServer(server2);
			});
			DomeinController.CloseConnectionToServer();
		}

		[TestMethod()]
		public void CloseConnectionToServer()
		{
			//Check that you can't close a database if there is none connected.
			DomeinController.AddServer(server2);
			DomeinController.OpenConnectionToServer(server2);
			DomeinController.CloseConnectionToServer();
			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.CloseConnectionToServer();
			});
			DomeinController.OpenConnectionToServer(server2);
			DomeinController.CloseConnectionToServer();
		}

		[TestMethod()]
		public void AddServerTest()
		{
			//Check that you can't add servers that are not valid
			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.AddServer(server1);
			});
			DomeinController.AddServer(server2);
		}

		[TestMethod()]
		public void RemoveServerTest()
		{
			//Check that you can't remove a server that does not exist.
			DomeinController.AddServer(server2);
			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.RemoveServer(server1);
			});

			DomeinController.RemoveServer(server2);
			DomeinController.AddServer(server2);
			DomeinController.OpenConnectionToServer(server2);

			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.RemoveServer(server2);
			});

			DomeinController.CloseConnectionToServer();
			DomeinController.RemoveServer(server2);
		}

		[TestMethod()]
		public void GetServersTest()
		{
			//Check that you get the inserted servers back.
			DomeinController.AddServer(server2);
			DomeinController.AddServer(server2);
			DomeinController.AddServer(server3);

			int serverCount = DomeinController.GetServers().Count();
			Debug.WriteLine($"Amount of servers: {serverCount}");

			if (serverCount != 2)
				Assert.Fail();
		}

		[TestMethod()]
		public void SqlQueryTest()
		{
			//Check if the query gives the correct reponse.
			DomeinController.AddServer(server2);
			DomeinController.OpenConnectionToServer(server2);
			Table result = DomeinController.SqlQuery("Show databases;");
			if (result.Rows.Count == 0)
				Assert.Fail();
		}

		[TestMethod()]
		public void UseDatabaseTest()
		{
			//Check that you can't use a non added database from the connected server.
			DomeinController.AddServer(server2);
			DomeinController.OpenConnectionToServer(server2);

			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.UseDatabase(database2);
			});
		}

		[TestMethod()]
		public void AddDatabaseTest()
		{
			//Check that you can't add the same database to a server.
			DomeinController.AddServer(server3);
			DomeinController.OpenConnectionToServer(server3);
			DomeinController.AddDatabase(database3);
			DomeinController.AddDatabase(database2);
			DomeinController.UseDatabase(database3);

			Assert.ThrowsException<DatabaseException>(() =>
			{
				DomeinController.AddDatabase(database3);
			});

			DomeinController.UseDatabase(database3);
		}

		[TestMethod()]
		public void RemoveDatabaseTest()
		{
			//Check you can't remove the same database you already removed.
			DomeinController.AddServer(server3);
			DomeinController.OpenConnectionToServer(server3);
			DomeinController.AddDatabase(database2);
			DomeinController.AddDatabase(database3);
			DomeinController.UseDatabase(database3);
			DomeinController.RemoveDatabase(database3);

			Assert.ThrowsException<DatabaseException>(() =>
			{
				var connectedDb = DomeinController.ConnectedDatabase;
				DomeinController.RemoveDatabase(database3);
			});
		}
	}
}