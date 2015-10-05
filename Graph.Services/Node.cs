using System.Collections.Generic;
using System.Linq;

namespace Graph.Services
{
	public class Node
	{
		public List<int> Adjacent { get; private set; }
		public int Id { get; set; }
		
		private readonly Graph _graph;

		/// <summary>
		/// Flag used by the traversal algorithm to set or determine whether the node had been already visited.
		/// </summary>
		internal bool IsExplored { get; set; }

		/// <summary>
		/// Traversal value.
		/// </summary>
		public int TraversalLayer { get; set; }

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
