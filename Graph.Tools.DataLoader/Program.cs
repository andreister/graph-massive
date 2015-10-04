using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Graph.Core.Logging;

namespace Graph.Tools.DataLoader
{
	class Program
	{
		private static readonly ILogger _logger = LogManager.GetLogger();

		static void Main(string[] args)
		{
			var folder = args.FirstOrDefault() ?? "";
			if (!Directory.Exists(folder)) {
				string exeName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
				_logger.Error(@"Usage: " + exeName + " <folder>\nPlease make sure the folder you specified does exist");
				return;
			}

			var xml = GetCombinedXml(folder);
		}

		private static XDocument GetCombinedXml(string folder)
		{
			var root = new XElement("nodes");
			
			foreach (var file in Directory.GetFiles(folder).Where(x => x.EndsWith(".xml")))
			{
				var xml = XDocument.Load(file);
				root.Add(xml.Descendants("node"));
			}

			var result = new XDocument();
			result.Add(root);

			return result;
		}
	}
}
