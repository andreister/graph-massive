using System.Collections.Generic;
using System.Linq;

namespace Graph.Services.Traversal
{
	public class Node
	{
		public List<int> Adjacent { get; private set; }
		public int Id { get; set; }
		
		private readonly Graph _graph;

		/// <summary>
		/// Flag used by the BFS algorithm to check if the node had been already visited.
		/// </summary>
		internal bool IsExplored { get; set; }

		/// <summary>
		/// Traversal layer updated by the BFS algorithm.
		/// </summary>
		public int Layer { get; set; }

		internal Node(int id, Graph graph)
		{
			IsExplored = false;

			Id = id;
			_graph = graph;

			Adjacent = new List<int>();
		}

		public IEnumerable<Node> GetUnexploredChildren()
		{
			return Adjacent.Select(x => _graph[x]).Where(x => !x.IsExplored);
		}

		public override string ToString()
		{
			return "[" + Id + " ~ (" + string.Join(",", Adjacent) + ")]";
		}
	}
}
