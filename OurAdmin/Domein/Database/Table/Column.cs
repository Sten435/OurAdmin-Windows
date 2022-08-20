using System;
using System.Collections.Generic;

namespace Domein.DataBase.DataTable
{

	public class Column
	{

		public string Name { get; set; }
		public bool? IsNull { get; set; }
		public int TypeAmount { get; set; }
		public string __Type { get; set; }
		public string SqlType { get; set; }
		public string Extra { get; set; }
		public string __LengthValues { get; set; } = string.Empty;
		public object __DefaultValue { get; set; } = string.Empty;
		public object __AsDefined { get; set; } = string.Empty;
		public string __Attributes { get; set; } = string.Empty;
		public string __Comments { get; set; } = string.Empty;
		public bool __AutoIncrement { get; set; } = false;


		public override bool Equals(object obj)
		{
			return obj is Column column &&
				   Name == column.Name &&
				   IsNull == column.IsNull &&
				   TypeAmount == column.TypeAmount &&
				   __Type == column.__Type &&
				   SqlType == column.SqlType &&
				   EqualityComparer<object>.Default.Equals(__DefaultValue, column.__DefaultValue) &&
				   Extra == column.Extra;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, IsNull, TypeAmount, __Type, SqlType, __DefaultValue, Extra);
		}
	}
}