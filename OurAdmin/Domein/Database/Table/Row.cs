using System;
using System.Collections.Generic;

namespace Domein.DataBase.DataTable {

	public class Row {
		public List<object> Items { get; set; }

		public Row(List<object> data) {
			this.Items = data;
		}

		public override bool Equals(object obj) {
			return obj is Row row &&
				   EqualityComparer<List<object>>.Default.Equals(Items, row.Items);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Items);
		}
	}
}