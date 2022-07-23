using System;

namespace Domein.DataBase.Sql.Exceptions {

	public class QueryException : Exception {

		public QueryException() {
		}

		public QueryException(string message) : base(message) {
		}
	}
}