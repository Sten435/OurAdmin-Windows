using Domein.DataBase.DataTable;
using Domein.DataBase.Exceptions;
using Domein.DataBase.Sql;
using Domein.DataBase.Sql.Exceptions;
using Domein.Validatie;
using System;
using System.Collections.Generic;

namespace Domein.DataBase {

	public class Database {
		public string Name { get; set; }
		public List<Table> Tables { get; }

		public override bool Equals(object obj) {
			return obj is Database database &&
				   Name == database.Name &&
				   EqualityComparer<List<Table>>.Default.Equals((List<Table>)Tables, (List<Table>)database.Tables);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Name, Tables);
		}

		public Database(string name, List<Table> tables) {
			this.Name = name.Trim();
			ValidateDatabase();
			this.Tables = tables;
		}

		private void ValidateDatabase() {
			if (Validate.NullOrWhiteSpace(Name)) throw new DatabaseException("Name can't be empty");
		}

		public QueryResult SqlQuery(string query) {
			if (Validate.NullOrWhiteSpace(query)) throw new QueryException("Query can't be empty.");
			return new QueryResult(query);
		}
	}
}