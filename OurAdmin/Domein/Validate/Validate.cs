using Domein.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Validatie {
	public class Validate {
		public static bool NullOrWhiteSpace(string input) => string.IsNullOrWhiteSpace(input);
		public static void ValidateDatabase<T>(T parameter, Action<T> callBack, string errorMessage) {
			if (parameter != null) callBack(parameter);
			else throw new DatabaseException(errorMessage);
		}
	}
}
