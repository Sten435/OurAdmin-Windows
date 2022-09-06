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
				   Extra == column.Extra &&
				   __LengthValues == column.__LengthValues &&
				   EqualityComparer<object>.Default.Equals(__DefaultValue, column.__DefaultValue) &&
				   EqualityComparer<object>.Default.Equals(__AsDefined, column.__AsDefined) &&
				   __Attributes == column.__Attributes &&
				   __Comments == column.__Comments &&
				   __AutoIncrement == column.__AutoIncrement;
		}

		public override int GetHashCode()
		{
			HashCode hash = new HashCode();
			hash.Add(Name);
			hash.Add(IsNull);
			hash.Add(TypeAmount);
			hash.Add(__Type);
			hash.Add(SqlType);
			hash.Add(Extra);
			hash.Add(__LengthValues);
			hash.Add(__DefaultValue);
			hash.Add(__AsDefined);
			hash.Add(__Attributes);
			hash.Add(__Comments);
			hash.Add(__AutoIncrement);
			return hash.ToHashCode();
		}
	}
}