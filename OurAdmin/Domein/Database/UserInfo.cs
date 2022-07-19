using System;

namespace Domein.Database {
	public class UserInfo : IConnectionValidator {
		public string User { get; set; }
		public string Password { get; set; }

		public void ValidateObject() {
			throw new NotImplementedException("Not implemented");
		}


	}

}
