using System.IO;
using System.Linq;
using System.Reflection;
using Graph.Core.Logging;
using Graph.Services.Common.Endpoints;

namespace Graph.Tools.DataLoader
{
	class Program
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		
		static void Main(string[] args)
		{
			var folder = args.FirstOrDefault() ?? "";
			if (!Directory.Exists(folder)) {
				var exeName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
				_logger.Error("Usage:\n\t" + exeName + " <folder>\n\tPlease make sure the folder you specified does exist\n");
				return;
			}

			var wcfManager = new ClientEndpointsManager();
			var graphProvider = new XmlGraphProvider(folder);
			var loader = new GraphLoader(wcfManager, graphProvider);
			
			loader.SaveGraph();
		}
	}
}
