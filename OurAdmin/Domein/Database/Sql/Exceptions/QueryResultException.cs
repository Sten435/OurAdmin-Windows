using System;

namespace Domein.DataBase.Sql.Exceptions {

	public class QueryResultException : Exception {

		public QueryResultException() {
		}

		public QueryResultException(string message) : base(message) {
		}
	}
}