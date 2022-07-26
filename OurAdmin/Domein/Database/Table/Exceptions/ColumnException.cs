using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Domein.DataBase.DataTable.Exceptions {

	public class ColumnException : Exception {

		public ColumnException() {
		}

		public ColumnException(string message) : base(message) {
		}
	}
}