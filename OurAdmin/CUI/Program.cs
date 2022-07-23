using Domein.Controllers;
using ReposInterface;
using Repository;
using System;

namespace CUI {

	public class Program {
		private static IServerInfo ServerRepo;
		private static DatabaseController databaseController;
		private static DomeinController domeinController;

		private static void Main() {
			ServerRepo = new ServerRepo();
			databaseController = new(ServerRepo);
			domeinController = new(databaseController);

			while (true) {
				Console.Clear();
				try {
					
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