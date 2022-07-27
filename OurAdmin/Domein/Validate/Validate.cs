using Domein.DataBase.Exceptions;
using System;

namespace Domein.Validatie {

	public class Validate {

		/// <summary>
		/// Validate if an object is null or contains only spaces.
		/// </summary>
		/// <param name="input">Object to validate</param>
		/// <returns>true or false respectfully if the validation was a success.</returns>
		public static bool NullOrWhiteSpace(string input) => string.IsNullOrWhiteSpace(input);

		/// <summary>
		/// Validate a database and add a callback to execute after validation.
		/// </summary>
		/// <typeparam name="T">Type of the object.</typeparam>
		/// <param name="parameter">Object to validate.</param>
		/// <param name="callBack">Callback to execute.</param>
		/// <param name="errorMessage">Error message when validation is not successful.</param>
		/// <exception cref="DatabaseException"></exception>
		public static void ValidateObject<T>(T parameter, Action<T> callBack, string errorMessage) {
			if (parameter != null) callBack(parameter);
			else throw new DatabaseException(errorMessage);
		}
	}
}