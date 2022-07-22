using Domein.DB;
using Domein.Exceptions;
using Domein.Validatie;
using System;
using System.Collections.Generic;

namespace Domein.DBConnectionInfo {
	public class Server {
		public string Host { get; set; }
		public string Port { get; set; }
		public List<Database> Databases { get; set; }

		private UserInfo _userInfo;

		public void Connect() {
			throw new NotImplementedException("Not implemented");
		}
		public void Close() {
			throw new NotImplementedException("Not implemented");
		}

		public override bool Equals(object obj) {
			return obj is Server server &&
				   Host == server.Host &&
				   Port == server.Port &&
				   EqualityComparer<List<Database>>.Default.Equals(Databases, server.Databases) &&
				   EqualityComparer<UserInfo>.Default.Equals(_userInfo, server._userInfo);
		}

		public override int GetHashCode() {
			return HashCode.Combine(Host, Port, Databases, _userInfo);
		}

		public Server(string host, UserInfo userInfo, List<Database> databases, string port = "3306") {
			this.Host = host.Trim();
			this.Port = port.Trim();
			ValidateServer();
			this._userInfo = userInfo;
			this.Databases = databases;
		}

		private void ValidateServer() {
			if (Validate.NullOrWhiteSpace(Host)) throw new DatabaseException("Host can't be empty");
			if (Validate.NullOrWhiteSpace(Port)) throw new DatabaseException("Port can't be empty");
		}
	}

}
