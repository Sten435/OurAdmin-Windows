using Domein.DataBase;
using Domein.DataBase.DataTable;
using System.Collections.Generic;
using System.Linq;

namespace GUI.Views.ViewClasses
{
	public class BreadCrumb
	{
		private Server Server { get; set; }
		private Database Database { get; set; }
		private string Table { get; set; }

		public string Content { get; set; }

		public BreadCrumb(Server server, Database database, string table)
		{
			Server = server;
			Database = database;
			Table = table;

			List<string> dataStringList = new List<string>() { Server?.Host.ToString(), Database?.Name.ToString(), Table ?? string.Empty };
			Content = string.Join(" > ", dataStringList.Where(str => !string.IsNullOrWhiteSpace(str)));
		}
	}
}