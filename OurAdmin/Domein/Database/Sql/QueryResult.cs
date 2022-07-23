using System;
using System.Collections.Generic;

namespace Domein.DataBase.Sql {

	public class QueryResult {
		public object Data { get; set; }

		public QueryResult(object data) {
			Data = data;
		}

		public override bool Equals(object obj) {
			return obj is QueryResult result &&
				   EqualityComparer<object>.Default.Equals(Data, result.Data);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Data);
		}
	}
}