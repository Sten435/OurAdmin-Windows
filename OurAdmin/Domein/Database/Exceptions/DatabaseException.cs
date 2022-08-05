using System;

namespace Domein.DataBase.Exceptions
{

	public class DatabaseException : Exception {

		public DatabaseException() {
		}

		public DatabaseException(string message) : base(message) {
		}
	}
}