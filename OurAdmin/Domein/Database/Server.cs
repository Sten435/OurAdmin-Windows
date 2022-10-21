using Domein.DataBase.Exceptions;
using Domein.Validatie;
using System;
using System.Collections.Generic;

namespace Domein.DataBase
{

	public class Server
	{
		public string Host { get; set; }
		public string Port { get; set; }
		public string ConnectionString { get; private set; }
		public HashSet<Database> Databases { get; set; } = new();

		public UserInfo UserInfo;

		public Server(string host, UserInfo userInfo, string port)
		{
			this.Host = host.Trim();
			this.Port = port.Trim();
			this.ConnectionString = $"Server={Host};User ID={userInfo.User};Password={userInfo.Password};Port={port}";
			ValidateServer();
			this.UserInfo = userInfo;
		}

		private void ValidateServer()
		{
			if (Validate.NullOrWhiteSpace(Host))
				throw new DatabaseException("Host can't be empty");
			if (Validate.NullOrWhiteSpace(Port))
				throw new DatabaseException("Port can't be empty");
		}

		public override bool Equals(object obj)
		{
			return obj is Server server &&
				   Host == server.Host &&
				   Port == server.Port &&
				   ConnectionString == server.ConnectionString &&
				   EqualityComparer<HashSet<Database>>.Default.Equals(Databases, server.Databases) &&
				   EqualityComparer<UserInfo>.Default.Equals(UserInfo, server.UserInfo);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Host, Port, ConnectionString, Databases, UserInfo);
		}
	}
}