using Domein.Database;
using System;

namespace Domein.DatabaseConnectionInfo {
	public class DatabaseConnectionInfo : IConnectionValidator {
		public string Server { get; set; }
		public string Port { get; set; }

		public void Connect() {
			throw new NotImplementedException("Not implemented");
		}
		public void Close() {
			throw new NotImplementedException("Not implemented");
		}
		public DatabaseConnectionInfo(string databaseName, string server, int port) {
			throw new NotImplementedException("Not implemented");
		}
		public void ValidateObject() {
			throw new NotImplementedException("Not implemented");
		}

		private UserInfo userInfo;
		private Database[] databases;


	}

}
