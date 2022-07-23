using System;

namespace Domein.DataBase.Exceptions {

	public class UserException : Exception {

		public UserException() {
		}

		public UserException(string message) : base(message) {
		}
	}
}