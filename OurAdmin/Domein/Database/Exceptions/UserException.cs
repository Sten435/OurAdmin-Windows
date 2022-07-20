using System;

namespace Domein.Exceptions {
	public class UserException : Exception {
		public UserException() {

		}
		public UserException(string message) : base(message) {

		}

	}

}
