using System.Collections;
using System.Collections.Generic;

namespace Graph.Services
{
	public class Graph : IEnumerable
	{
		private readonly Dictionary<int, Node> _graph = new Dictionary<int, Node>();

		public Node this[int id]
		{
			get { return _graph[id]; }
		}

		/// <summary>
		/// Duck typing to simplify initialization in unit tests :/ 
		/// Have to stick with "Add" as the method name.
		/// </summary>
		public void Add(int from, int to)
		{
			RegisterNode(from);
			RegisterNode(to);

			_graph[from].Adjacent.Add(to);
		}

		private void RegisterNode(int id)
		{
			if (!_graph.ContainsKey(id)) {
				_graph.Add(id, new Node(id, this));
			}
		}

		public IEnumerator GetEnumerator()
		{
			return _graph.GetEnumerator();
		}
	}
}
