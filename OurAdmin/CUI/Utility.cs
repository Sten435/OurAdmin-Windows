using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUI {
	internal class Utility {

		private static readonly string _predixSpace = "    ";

		public static readonly string SelectPrefix = "|-> ";

		public static int SelectedIndex { get; private set; }
		public static List<string> GaVerderOpties { get; private set; }
		public static List<string> StopOpties { get; private set; }
		public static List<string> DisabledOptie { get; private set; }

		private static void DisplayOptions(List<string> optieLijst, bool metEinde = false, bool metPijl = true) {
			Console.ResetColor();

			for (int i = 0; i < optieLijst.Count; i++)
				if (i == SelectedIndex) {
					string text;
					text = $"{SelectPrefix}{optieLijst[i]}";

					if (!metPijl) text = $"{optieLijst[i]}";

						Console.ForegroundColor = ConsoleColor.White;

						if (!metPijl) {
							Console.BackgroundColor = ConsoleColor.Black;
							Console.ForegroundColor = ConsoleColor.White;
						}

					Console.WriteLine(text);
					Console.ResetColor();
				} else {
					string text;
					text = optieLijst[i];

					Console.ResetColor();

					if (!metPijl) Console.WriteLine(text);
					else Console.WriteLine(@$"{_predixSpace}{optieLijst[i]}");
				}

			Console.ResetColor();
		}

		public static int OptieLijstConroller(List<string> optieLijst, string prompt = "", bool metEinde = false, bool metPijl = true) {
			ConsoleKey consoleKey;

			Console.CursorVisible = false;
			Console.Clear();

			do {
				Logger.Info(prompt);
				DisplayOptions(optieLijst, metEinde: metEinde, metPijl: metPijl);

				consoleKey = Console.ReadKey().Key;

				if (consoleKey == ConsoleKey.UpArrow && SelectedIndex > 0) SelectedIndex--;
				else if (consoleKey == ConsoleKey.DownArrow && SelectedIndex < optieLijst.Count - 1) SelectedIndex++;

				Console.Clear();
			} while (consoleKey != ConsoleKey.Enter);

			Console.CursorVisible = true;
			return SelectedIndex;
		}

		public static void ResetIndex() {
			SelectedIndex = 0;
		}

		public static class Logger {

			public static void Error(Exception error, bool newLine = true, bool metAchtergrond = true, bool clearConsole = false, bool metKeyPress = true) {
				Console.ResetColor();

				if (clearConsole) Console.Clear();

				if (metAchtergrond) {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Red;
				} else Console.ForegroundColor = ConsoleColor.Red;

				if (newLine) Console.WriteLine($"{error.Message}\n");
				else Console.Write($"{error.Message}");

				Console.ResetColor();

				if (metKeyPress) AskUser.ReadKnop();
			}

			public static void Error(string message, bool newLine = true, bool metAchtergrond = true, bool clearConsole = false, bool metKeyPress = true) {
				if (newLine) Error(new Exception(message), true, metAchtergrond, clearConsole: clearConsole, metKeyPress: metKeyPress);
				else Error(new Exception(message), false, metAchtergrond, clearConsole: clearConsole, metKeyPress: metKeyPress);
			}

			public static void Info(Exception error, bool newLine = true, bool metAchtergrond = true, ConsoleColor color = ConsoleColor.Black) {
				Console.ResetColor();
				if (metAchtergrond) {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.ForegroundColor = ConsoleColor.Cyan;
				} else
					Console.ForegroundColor = ConsoleColor.Cyan;

				if (color != ConsoleColor.Black)
					Console.ForegroundColor = color;

				if (newLine)
					Console.WriteLine($"{error.Message}\n");
				else
					Console.Write($"{error.Message}");
				Console.ResetColor();
			}

			public static void Info(string message, bool newLine = true, bool metAchtergrond = true, ConsoleColor color = ConsoleColor.Black) {
				if (newLine) Info(new Exception(message), true, metAchtergrond, color: color);
				else Info(new Exception(message), false, metAchtergrond, color: color);
			}
		}

		public static class AskUser {

			public static string ReadInput(ConsoleColor color = ConsoleColor.White, string prompt = "", bool metAchtergrond = false, ConsoleColor promptColor = ConsoleColor.White) {
				string input;

				Console.ForegroundColor = ConsoleColor.Cyan;
				Logger.Info(prompt, false, metAchtergrond: metAchtergrond, color: promptColor);

				if (color == ConsoleColor.White) Console.ForegroundColor = ConsoleColor.Cyan;
				else Console.ForegroundColor = color;

				Logger.Info(" ", false, metAchtergrond: false);
				input = Console.ReadLine();

				Console.ResetColor();
				return input;
			}

			public static ConsoleKey ReadKnop(ConsoleColor color = ConsoleColor.White, string prompt = "Klik op een knop om verder te gaan.", ConsoleColor promptColor = ConsoleColor.White) {
				ConsoleKey key;

				Console.CursorVisible = false;
				Console.ForegroundColor = ConsoleColor.Cyan;

				Logger.Info(prompt, false, false, color: promptColor);
				Console.ForegroundColor = color;

				key = Console.ReadKey(false).Key;
				if (color != ConsoleColor.White) Console.ResetColor();
				Console.CursorVisible = true;

				return key;
			}
		}

	}
}
