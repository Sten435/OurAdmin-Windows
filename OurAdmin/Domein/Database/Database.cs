using Domein.DataBase.Exceptions;
using Domein.Validatie;
using System;

namespace Domein.DataBase
{

	public class Database
	{
		public string Name { get; set; }

		public Database(string name)
		{
			this.Name = name.Trim();
			ValidateDatabase();
		}

		private void ValidateDatabase()
		{
			if (Validate.NullOrWhiteSpace(Name))
				throw new DatabaseException("Name can't be empty");
		}

		public override string ToString()
		{
			return $"{Name}";
		}

		public override bool Equals(object obj)
		{
			return obj is Database database &&
				   Name == database.Name;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name);
		}
	}

	public enum DatabaseType
	{
		MYSQL,
		SQL_SERVER
	}
}