using System;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;
using Graph.Services.Common.Endpoints;

namespace Graph.Tools.DataLoader
{
	internal class GraphLoader
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		private readonly IClientEndpointsManager _manager;
		private readonly IGraphProvider _graphProvider;

		public GraphLoader(IClientEndpointsManager manager, IGraphProvider graphProvider)
		{
			_manager = manager;
			_graphProvider = graphProvider;
		}

		internal void SaveGraph()
		{
			try
			{
				var service = _manager.GetService<IStorageService>();
				var xml = _graphProvider.GetGraph();

				service.SaveGraph(xml);
			}
			catch (Exception ex)
			{
				_logger.Error("Unexpected exception while saving the graph", ex);
			}
		}
	}
}
