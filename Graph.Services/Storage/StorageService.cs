using System.Xml.Linq;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;

namespace Graph.Services.Storage
{
	internal class StorageService : IStorageService
	{
		private static readonly ILogger _logger = LogManager.GetLogger();

		public void SaveGraph(XElement nodes)
		{
			_logger.Debug("Received XML: " + nodes.Name);
			_logger.Debug("If you see this, WCF is working!");
		}
	}
}
