using Domein.Controllers;
using Domein.DataBase;
using ReposInterface;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CUI
{

	public class Program
	{
		private static IServerInfo ServerRepo;
		private static DatabaseController databaseController;
		private static DatabaseType databaseType;
		private static void Main()
		{
			databaseType = DatabaseType.MYSQL;
			while (true)
			{
				Utility.ResetIndex();
				ServerRepo = new ServerRepo(databaseType);
				databaseController = new(ServerRepo, databaseType);
				DomeinController.DatabaseController = databaseController;

				string hostNaam;
				UserInfo userInfo;
				Server server;
				Server server2;

				Console.Clear();

				try
				{
					hostNaam = GetHostNaam();
					userInfo = GetUserInfo();
					server = new(hostNaam, userInfo);
					server2 = new("testing server 2", userInfo);

					DomeinController.AddServer(server);
					DomeinController.AddServer(server2);
					DomeinController.OpenConnectionToServer(server);

					SelectDB();

					while (true)
					{
						//Table dataList = DomeinController.SqlQuery(Utility.AskUser.ReadInput(prompt: "Type a query: ", promptColor: ConsoleColor.Cyan));

						//Console.WriteLine(DomeinController.ToJson(dataList));

						//for (int i = 0; i < dataList.Rows.Count; i++)
						//{
						//	for (int col = 0; col < dataList.Columns.Count; col++)
						//	{
						//		Console.WriteLine($"Name: {dataList.Rows[i].Items[col]}");
						//		Console.WriteLine($"IsNull: {dataList.Columns.ToList()[col].IsNull}");
						//		Console.WriteLine($"Type: {dataList.Columns.ToList()[col].Type}");
						//		Console.WriteLine($"SqlType: {dataList.Columns.ToList()[col].SqlType}");
						//		Console.WriteLine($"TypeAmount: {dataList.Columns.ToList()[col].TypeAmount}");
						//		Console.WriteLine($"DefaultValue: {dataList.Columns.ToList()[col].DefaultValue}");
						//		Console.WriteLine($"AutoIncrement: {dataList.Columns.ToList()[col].AutoIncrement}");
						//		Console.WriteLine("------------------------------");
						//	}
						//}

						Utility.AskUser.ReadKnop();
						Console.Clear();
					}
				} catch (Exception error)
				{
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine(error.Message);
					Console.ResetColor();
				}
				Utility.AskUser.ReadKnop();
			}

			static string GetHostNaam()
			{
				List<string> hostsList = new() { "localhost" };
				int selectedHostIndex = Utility.OptieLijstConroller(hostsList, prompt: "Choose a server");
				return hostsList[selectedHostIndex];
			}

			static UserInfo GetUserInfo()
			{
				string user = Utility.AskUser.ReadInput(prompt: "Enter a user");
				string password = Utility.AskUser.ReadInput(prompt: "Enter a password");
				return new UserInfo(user, password);
			}

			void SelectDB()
			{
				List<string> databases = DomeinController.GetDatabasesFromServer().Select(db => db.Name).ToList();
				int selectedDatabase = Utility.OptieLijstConroller(databases, prompt: "Choose a database");
				string databaseName = databases[selectedDatabase];

				Database database = new(databaseName);
				Database databaseOne = new(databaseName);

				DomeinController.AddDatabase(database);
				DomeinController.UseDatabase(databaseOne);
			}
		}
	}
}