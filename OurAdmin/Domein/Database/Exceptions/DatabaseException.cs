using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Domein.DataBase.Exceptions {

	public class DatabaseException : Exception {

		public DatabaseException() {
		}

		public DatabaseException(string message) : base(message) {
		}
	}
}