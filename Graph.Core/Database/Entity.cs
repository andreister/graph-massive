namespace Graph.Core.Database
{
	public abstract class Entity
	{
		public int Id { get; set; }

		public abstract string SaveSql { get; }
	}
}
