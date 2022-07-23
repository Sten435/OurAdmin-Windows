using System;

namespace Domein.DataBase.DataTable.Exceptions {

	public class ColumnException : Exception {

		public ColumnException() {
		}

		public ColumnException(string message) : base(message) {
		}
	}
}