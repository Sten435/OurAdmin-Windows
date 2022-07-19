using DCD;
using Domein.Database.Sql.Intefaces;
using System;

namespace Domein.Database.Sql {
	public class SqlQuery : IQuery {
		public string Query { get; set; }

		public QueryResult RunQuery(string query) {
			throw new NotImplementedException("Not implemented");
		}

		private QueryResult queryResult;

	}

}
