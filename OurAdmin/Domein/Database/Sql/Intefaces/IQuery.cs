using DCD;
using System;

namespace Domein.Database.Sql.Intefaces {
	public interface IQuery {
		QueryResult RunQuery(string query);

	}

}
