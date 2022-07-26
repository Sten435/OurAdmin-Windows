﻿using Domein.DataBase.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Views.ViewClasses
{
	public class StructureTableViewClass
	{
		public string Name { get; }
		public string Type { get; }
		public bool? Null { get; }
		public string Extra { get; }
		public readonly Column Column;

		public StructureTableViewClass(string name, string type, bool? nul, string extra, Column column)
		{
			Name = name;
			Type = type;
			Null = nul;
			Extra = extra;
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
				   Type == @class.Type &&
				   Null == @class.Null &&
				   Extra == @class.Extra &&
				   EqualityComparer<Column>.Default.Equals(Column, @class.Column);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Type, Null, Extra, Column);
		}
	}
}
