using Domein.Validatie;
using Domein.DB.Enums;
using Domein.DB.Exceptions;
using Domein.Interfaces;
using System;
using System.Collections.Generic;

namespace Domein.DB {
	public class Column {
		public string Name { get; set; }
		public bool IsNull { get; set; }
		public string TypeAmount { get; set; }

		public EDatabaseType Type { get; set; }
		public EDatabaseExtra Extra { get; set; }
		public IKey Key { get; set; }

		public override bool Equals(object obj) {
			return obj is Column column &&
				   Name == column.Name &&
				   IsNull == column.IsNull &&
				   TypeAmount == column.TypeAmount &&
				   Type == column.Type &&
				   Extra == column.Extra &&
				   EqualityComparer<IKey>.Default.Equals(Key, column.Key);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Name, IsNull, TypeAmount, Type, Extra, Key);
		}

		public Column(string name, bool isNull, string typeAmount, EDatabaseType type, EDatabaseExtra extra, IKey key) {
			this.Name = name.Trim();
			this.TypeAmount = typeAmount.Trim();
			ValidateColumn();
			this.IsNull = isNull;
			this.Type = type;
			this.Extra = extra;
			this.Key = key;
		}

		private void ValidateColumn() {
			if (Validate.NullOrWhiteSpace(Name)) throw new ColumnException("Column name can't be empty");
			if (Validate.NullOrWhiteSpace(TypeAmount)) throw new ColumnException("Column typeAmount can't be empty");
		}
	}

}
