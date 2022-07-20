using Domein.Controllers;
using Domein.DB;
using Domein.DBConnectionInfo;
using ReposInterface;
using Repository;
using System;
using System.Collections.Generic;

namespace CUI {
	public class Program {
		private static IDatabaseConInfo databaseConnectionInfoRepo;
		private static DatabaseController databaseController;
		private static DomeinController domeinController;

		static void Main() {
			databaseConnectionInfoRepo = new DatabaseConnectionInfoRepo();
			databaseController = new(databaseConnectionInfoRepo);
			domeinController = new(databaseController);

			while (true) {
				Console.Clear();
				try {
					//Test
				} catch (Exception error) {
					Console.BackgroundColor = ConsoleColor.DarkRed;
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine(error.Message);
					Console.ResetColor();
				}
				Console.ReadKey(false);
			}
		}
	}
}