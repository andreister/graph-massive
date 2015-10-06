using System;
using System.Collections.Generic;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;
using System.Linq;

namespace Graph.Services.Traversal
{
	internal class TraversalService : ITraversalService
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		private readonly ILoadGraphRepository _repository;

		public TraversalService()
		{
			_repository = new LoadGraphRepository();
		}

		public TraversalService(ILoadGraphRepository repository)
		{
			_repository = repository;
		}

		public int[] FindShortestPath(int fromId, int toId)
		{
			try {
				var graph = _repository.LoadGraph();
				
				var startNode = graph[fromId];
				startNode.Layer = 0;

				var algorithm = new BreadthFirstSearch();
				algorithm.Run(startNode, x => {
					foreach (var child in x.GetUnexploredChildren()) {
						child.Layer = x.Layer + 1;
						if (child.Id == toId) {
							algorithm.Stop = true;
						}
					}
				});

				return GetPath(fromId, toId, graph);
			}
			catch (Exception ex) {
				_logger.Error("Unexpected exception, cannot calculate the shortest path", ex);
				throw;
			}
		}

		/// <summary>
		/// Recursively finds the shortest path by backtracing from "toId" up to the "fromId" node.
		/// </summary>
		private int[] GetPath(int fromId, int toId, Graph graph)
		{
			var path = new Stack<int>();
			path.Push(toId);

			var node = graph[toId];
			
			var upperIds = node.Adjacent.Where(x => graph[x].Layer == node.Layer - 1);
			foreach (var upperId in upperIds)
			{
				if (upperId == fromId) {
					path.Push(fromId);
					break;
				}

				var subsetPath = GetPath(fromId, upperId, graph);
				if (subsetPath[0] == fromId) {
					for (int i = subsetPath.Length - 1; i >= 0; i--) {
						path.Push(subsetPath[i]);
					}
					break;
				}
			}

			return path.ToArray();
		}
	}
}
