using Graph.Core.Database.Generated;
using Graph.Core.Database.Repositories;

namespace Graph.Services.Traversal
{
	internal class LoadGraphRepository : Repository, ILoadGraphRepository
	{
		public Graph LoadGraph()
		{
			var edges = Select<EdgeEntity>();

			var result = new Graph();
			foreach (var edge in edges) {
				result.Add(edge.From, edge.To);
			}
			return result;
		}
	}
}