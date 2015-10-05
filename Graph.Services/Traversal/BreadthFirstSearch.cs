using System;
using System.Collections.Generic;

namespace Graph.Services.Traversal
{
	internal class BreadthFirstSearch
	{
		public bool Stop { get; set; }

		public void Run(Node startNode, Action<Node> process)
		{
			var queue = new Queue<Node>();
			queue.Enqueue(startNode);

			while (queue.Count != 0) {
				var node = queue.Dequeue();
				if (node.IsExplored) {
					continue;
				}

				foreach (var child in node.GetUnexploredChildren()) {
					queue.Enqueue(child);
				}

				node.IsExplored = true;
				process(node);

				if (Stop) {
					break;
				}
			}
		}
	}
}
