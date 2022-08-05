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
		private Table Table { get; set; }

		public string Content { get; set; }

		public BreadCrumb(Server server, Database database, Table table)
		{
			Server = server;
			Database = database;
			Table = table;

			List<string> dataStringList = new List<string>() { Server?.Host.ToString(), Database?.Name.ToString(), Table?.Name.ToString() };
			Content = string.Join(" > ", dataStringList.Where(str => !string.IsNullOrWhiteSpace(str)));

		}
	}
}