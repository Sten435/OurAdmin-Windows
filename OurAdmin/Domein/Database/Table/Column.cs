using Domein.Database.Table.Enums;
using Domein.Interfaces;
using System;

namespace Domein.Database.Table {
	public class Column {
		public string Name { get; set; }
		public bool IsNull { get; set; }
		public int TypeAmount { get; set; }

		public Column(string _name, bool _isNull, int _typeAmount, IKey _key, EDatabaseType _type, EDatabaseExtra extra) {
			throw new NotImplementedException("Not implemented");
		}

		private EDatabaseType type;
		private EDatabaseExtra extra;
		private IKey key;


	}

}
