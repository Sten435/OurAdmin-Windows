using System;

namespace Domein.Exceptions {
	public class DatabaseException : Exception {
		public DatabaseException() {
		}
		public DatabaseException(string message) : base(message) {
		}

	}

}
