using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Domein.DataBase.DataTable
{

	public class Table
	{
		public string Name { get; set; }
		public List<Row> Rows { get; }
		public HashSet<Column> Columns { get; }
		public DataRelationCollection Relations { get; set; }

		public Table()
		{
			this.Rows = new();
			this.Columns = new();
		}

		public void AddRow(Row row) => Rows.Add(row);

		public bool RemoveRow(Row row) => Rows.Remove(row);

		public void AddColumn(Column column) => Columns.Add(column);

		public void RemoveColumn(Column column) => Columns.Remove(column);

		public override bool Equals(object obj)
		{
			return obj is Table table &&
				   EqualityComparer<List<Row>>.Default.Equals(Rows, table.Rows) &&
				   EqualityComparer<HashSet<Column>>.Default.Equals(Columns, table.Columns);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Rows, Columns);
		}
	}
}