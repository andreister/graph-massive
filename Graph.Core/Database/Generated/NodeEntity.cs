﻿namespace Graph.Core.Database.Generated
{
	//This code is autogenerated

	[TableName("Nodes")]
	public class NodeEntity : Entity
	{
		public string Label { get; set; }

		public override string SaveSql
		{
			get
			{
				return @"
					IF EXISTS (SELECT * FROM Nodes WHERE Id=@Id)
					BEGIN
						UPDATE Nodes SET Label=@Label WHERE Id=@Id
						SELECT 0 AS NewId
					END
					ELSE 
					BEGIN
						INSERT INTO Nodes (Id, Label) VALUES (@Id, @Label)
						SELECT 0 AS NewId
					END
				";
			}
		}
	}
}
