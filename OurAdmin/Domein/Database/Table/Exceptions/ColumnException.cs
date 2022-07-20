using System;

namespace Domein.DB.Exceptions {
	public class ColumnException : Exception {
		public ColumnException() {

		}
		public ColumnException(string message) : base(message) {

		}

	}

}
