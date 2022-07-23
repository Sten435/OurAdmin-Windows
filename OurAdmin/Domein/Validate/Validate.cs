using Domein.DataBase.Exceptions;
using System;

namespace Domein.Validatie {

	public class Validate {

		public static bool NullOrWhiteSpace(string input) => string.IsNullOrWhiteSpace(input);

		public static void ValidateDatabase<T>(T parameter, Action<T> callBack, string errorMessage) {
			if (parameter != null) callBack(parameter);
			else throw new DatabaseException(errorMessage);
		}
	}
}