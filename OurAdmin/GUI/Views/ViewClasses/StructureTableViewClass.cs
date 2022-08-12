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

		public StructureTableViewClass(string name, string type, bool? nul, string extra)
		{
			Name = name;
			Type = type;
			Null = nul;
			Extra = extra;
		}

		public StructureTableViewClass(string name)
		{
			Name = name;
		}
	}
}
