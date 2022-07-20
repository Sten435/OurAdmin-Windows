using System;
using System.Collections.Generic;

namespace Domein.DB {
	public class Row {
		public List<string> Data { get; set; }

		public override bool Equals(object obj) {
			return obj is Row row &&
				   EqualityComparer<List<string>>.Default.Equals(Data, row.Data);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Data);
		}

		public Row(List<string> data) {
			this.Data = data;
		}
	}

}
