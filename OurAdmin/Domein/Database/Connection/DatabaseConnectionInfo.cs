using Domein.DB;
using Domein.Exceptions;
using Domein.Validatie;
using System;
using System.Collections.Generic;

namespace Domein.DBConnectionInfo {
	public class DatabaseConnectionInfo {
		public string Server { get; set; }
		public string Port { get; set; }
		public List<Database> Databases { get; set; }

		private UserInfo _userInfo;

		public void Connect() {
			throw new NotImplementedException("Not implemented");
		}
		public void Close() {
			throw new NotImplementedException("Not implemented");
		}

		public DatabaseConnectionInfo(string server, UserInfo userInfo, List<Database> databases, string port = "3306") {
			this.Server = server.Trim();
			this.Port = port.Trim();
			ValidateDatabaseConnectionInfo();
			this._userInfo = userInfo;
			this.Databases = databases;
		}

		public override bool Equals(object obj) {
			return obj is DatabaseConnectionInfo info &&
				   Server == info.Server &&
				   Port == info.Port &&
				   EqualityComparer<UserInfo>.Default.Equals(_userInfo, info._userInfo) &&
				   EqualityComparer<List<Database>>.Default.Equals(Databases, info.Databases);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Server, Port, _userInfo, Databases);
		}

		private void ValidateDatabaseConnectionInfo() {
			if (Validate.NullOrWhiteSpace(Server)) throw new DatabaseException("Server can't be empty");
			if (Validate.NullOrWhiteSpace(Port)) throw new DatabaseException("Port can't be empty");
		}
	}

}
