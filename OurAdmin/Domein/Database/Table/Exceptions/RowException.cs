using System;

namespace Domein.DataBase.DataTable.Exceptions {

	public class RowException : Exception {

		public RowException() {
		}

		public RowException(string message) : base(message) {
		}
	}
}