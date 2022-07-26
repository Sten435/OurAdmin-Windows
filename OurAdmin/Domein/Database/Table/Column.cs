using System;
using System.Collections.Generic;

namespace Domein.DataBase.DataTable {

	public class Column {
		public string Name { get; set; }
		public bool IsNull { get; set; }
		public int TypeAmount { get; set; }
		public string Type { get; set; }
		public string SqlType { get; set; }
		public object DefaultValue { get; set; }
		public bool AutoIncrement { get; set; }

		public override bool Equals(object obj) {
			return obj is Column column &&
				   Name == column.Name &&
				   IsNull == column.IsNull &&
				   TypeAmount == column.TypeAmount &&
				   Type == column.Type &&
				   SqlType == column.SqlType &&
				   EqualityComparer<object>.Default.Equals(DefaultValue, column.DefaultValue) &&
				   AutoIncrement == column.AutoIncrement;
		}

		public override int GetHashCode() {
			return HashCode.Combine(Name, IsNull, TypeAmount, Type, SqlType, DefaultValue, AutoIncrement);
		}
	}
}