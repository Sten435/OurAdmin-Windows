using Domein.DataBase.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Views.ViewClasses
{
	public class SqlQueryViewTable
	{
		public readonly string Name;
		public readonly Column Column;

		public SqlQueryViewTable(string name, Column column)
		{
			Name = name;
			Column = column;
		}

		//public StructureTableViewClass(string name)
		//{
		//	Name = name;
		//}

		public override bool Equals(object obj)
		{
			return obj is StructureTableViewClass @class &&
				   Name == @class.Name &&
				   EqualityComparer<Column>.Default.Equals(Column, @class.Column);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Column);
		}
	}
}
