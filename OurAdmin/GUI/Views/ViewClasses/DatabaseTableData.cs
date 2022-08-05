using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Views.ViewClasses
{
	public class DatabaseTableData
	{
		public string Table { get; set; }
		public string Actie { get; set; } = "Browse";
		public string Rows { get; set; } = "Empty";
		public string Size { get; set; } = "Drop";

		public DatabaseTableData(string table)
		{
			Table = table;
		}
	}
}
