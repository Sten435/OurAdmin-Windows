using System;

namespace Domein.DataBase.Sql.Exceptions
{

	public class QueryException : Exception {
		public string Query { get; private set; }

		public QueryException() {
		}

		public QueryException(string message, string query) : base(message) {
			Query = query;
		}
	}
}