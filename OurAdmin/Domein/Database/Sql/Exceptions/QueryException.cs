using System;

namespace Domein.Exceptions {
	public class QueryException : Exception {
		public QueryException() {

		}
		public QueryException(string message) : base(message) {

		}

	}

}
