using System;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;

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

		public int FindShortestPath(int fromId, int toId)
		{
			try {
				var graph = _repository.LoadGraph();
				
				var startNode = graph[fromId];
				startNode.DistanceFromStart = 0;

				var algorithm = new BreadthFirstSearch();
				algorithm.Run(startNode, x => {
					foreach (var child in x.GetUnexploredChildren()) {
						child.DistanceFromStart = x.DistanceFromStart + 1;
						if (child.Id == toId) {
							algorithm.Stop = true;
						}
					}
				});

				return graph[toId].DistanceFromStart;
			}
			catch (Exception ex) {
				_logger.Error("Unexpected exception, cannot calculate the shortest path", ex);
				throw;
			}
		}
	}
}
