using System;
using System.Transactions;
using System.Xml.Linq;
using Graph.Core.Database.Generated;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;
using Graph.Services.Storage.Extensions;

namespace Graph.Services.Storage
{
	internal class StorageService : IStorageService
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		private readonly ISaveGraphRepository _repository;

		public StorageService()
		{
			_repository = new SaveGraphRepository();
		}

		public StorageService(ISaveGraphRepository repository)
		{
			_repository = repository;
		}

		public void SaveGraph(XElement nodes)
		{
			try {
				using (var scope = new TransactionScope()) {
					UpdateGraph(nodes);

					scope.Complete();
				}
			}
			catch (Exception ex) {
				_logger.Error("Unexpected exception, the graph was not saved", ex);
				throw;
			}
		}

		private void UpdateGraph(XElement nodes)
		{
			_repository.DeleteExistingGraph();

			foreach (var node in nodes.Descendants("node")) {
				var entity = new NodeEntity {
					Id = node.GetElementValue<int>("id"),
					Label = node.GetElementValue<string>("label")
				};
				_repository.Save(entity);
			}

			foreach (var node in nodes.Descendants("node")) {
				var from = node.GetElementValue<int>("id");
				
				var adjacentNodes = node.Element("adjacentNodes");
				if (adjacentNodes == null) {
					continue;
				}

				foreach (var adjacent in adjacentNodes.Descendants("id")) {
					var entity = new EdgeEntity {
						From = from,
						To = adjacent.GetElementValue<int>("id")
					};
					_repository.Save(entity);
				}
			}
		}
	}

	namespace Extensions
	{
		internal static class XElementExtensions
		{
			internal static TResult GetElementValue<TResult>(this XElement node, string elementName)
			{
				var element = node.Element(elementName);
				if (element == null) {
					return default(TResult);
				}

				var text = element.Value;

				var conversionType = typeof(TResult);

				if (conversionType == typeof(int)) {
					int result;
					return (TResult)Convert.ChangeType(int.TryParse(element.Value, out result) ? result : 0, conversionType);
				}
				if (conversionType == typeof(string)) {
					return (TResult)Convert.ChangeType(text, conversionType);
				}

				throw new Exception("Unsupported type: " + conversionType);
			}
		}
	}
}
