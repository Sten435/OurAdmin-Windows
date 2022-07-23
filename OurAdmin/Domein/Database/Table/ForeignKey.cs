using Domein.DataBase.DataTable.Interfaces;
using System;
using System.Collections.Generic;

namespace Domein.DataBase.DataTable {

	public class ForeignKey : IKey {
		public Column Column { get; set; }

		public override bool Equals(object obj) {
			return obj is ForeignKey key &&
				   EqualityComparer<Column>.Default.Equals(Column, key.Column);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Column);
		}

		public ForeignKey(Column column) {
			this.Column = column;
		}
	}
}