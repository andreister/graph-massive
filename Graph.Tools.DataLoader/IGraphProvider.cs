using System.Xml.Linq;

namespace Graph.Tools.DataLoader
{
	public interface IGraphProvider
	{
		XElement GetGraph();
	}
}