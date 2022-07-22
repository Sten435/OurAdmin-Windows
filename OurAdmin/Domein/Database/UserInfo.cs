using System;
using Domein.Exceptions;
using Domein.Validatie;

namespace Domein.DB {
	public class UserInfo {
		public string User { get; set; }
		public string Password { get; set; }

		public UserInfo(string user, string password) {
			this.User = user.Trim();
			this.Password = password.Trim();
			ValidateUserInfo();
		}

		private void ValidateUserInfo() {
			if (Validate.NullOrWhiteSpace(User)) throw new UserException("Username can't be empty");
		}
	}

}
