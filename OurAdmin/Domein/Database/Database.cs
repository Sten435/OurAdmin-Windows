using DCD;
using System;

namespace Domein.Database {
	public class Database {
		public int Id { get; set; }
		public string Name { get; set; }

		private Table[] tables;
	}

}
