using System;

namespace Graph.Core.Database
{
	public class TableNameAttribute : Attribute
	{
		public string TableName { get; private set; }

		public TableNameAttribute(string tableName)
		{
			TableName = tableName;
		}
	}
}