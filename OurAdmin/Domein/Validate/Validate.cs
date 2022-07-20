using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domein.Validatie {
	public class Validate {
		public static bool NullOrWhiteSpace(string input) => string.IsNullOrWhiteSpace(input);
	}
}
