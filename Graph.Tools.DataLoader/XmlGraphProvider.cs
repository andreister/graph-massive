using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Graph.Tools.DataLoader
{
	internal class XmlGraphProvider : IGraphProvider
	{
		private readonly string _folder;

		public XmlGraphProvider(string folder)
		{
			_folder = folder;
		}

		public XElement GetGraph()
		{
			var root = new XElement("nodes");

			foreach (var file in Directory.GetFiles(_folder).Where(x => x.EndsWith(".xml")))
			{
				var xml = XDocument.Load(file);
				root.Add(xml.Descendants("node"));
			}

			return root;
		}
	}
}