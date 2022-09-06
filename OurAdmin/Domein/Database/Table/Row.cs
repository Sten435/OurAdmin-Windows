using System;
using System.Collections.Generic;

namespace Domein.DataBase.DataTable {

	public class Row {
		public List<string> Items { get; set; }

		public Row(List<string> data) {
			this.Items = data;
		}

		public override bool Equals(object obj) {
			return obj is Row row &&
				   EqualityComparer<List<string>>.Default.Equals(Items, row.Items);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Items);
		}
	}
}