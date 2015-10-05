using System;
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
		private readonly IStorageRepository _repository;

		public StorageService()
		{
			_repository = new StorageRepository();
		}

		public StorageService(IStorageRepository repository)
		{
			_repository = repository;
		}

		public void SaveGraph(XElement nodes)
		{
			try {
				_repository.DeleteExistingGraph();

				foreach (var node in nodes.Descendants("node")) {
					var entity = new Node {
						Id = node.GetElementValue<int>("id"), 
						Label = node.GetElementValue<string>("label")
					};
					_repository.Save(entity);
				}
			}
			catch (Exception ex) {
				_logger.Error("Unexpected exception", ex);
				throw;
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
