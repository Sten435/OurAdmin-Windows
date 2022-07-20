using System;
using System.Collections.Generic;

namespace Domein.DB {
	public class Table {
		public List<Row> Rows { get; }
		public List<Column> Columns { get; }

		public void GenerateTable() {
			throw new System.NotImplementedException("Not implemented");
		}
		public void AddRow(Row row) => Rows.Add(row);
		public bool RemoveRow(Row row) => Rows.Remove(row);
		public void AddColumn(Column column) => Columns.Add(column);
		public void RemoveColumn(Column column) => Columns.Remove(column);

		public override bool Equals(object obj) {
			return obj is Table table &&
				   EqualityComparer<List<Row>>.Default.Equals(Rows, table.Rows) &&
				   EqualityComparer<List<Column>>.Default.Equals(Columns, table.Columns);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Rows, Columns);
		}

		public Table() {
			this.Rows = new();
			this.Columns = new();
		}
	}

}
