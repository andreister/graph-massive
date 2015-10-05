using System.Collections.Generic;
using Graph.Services.Common.Definitions;

namespace Graph.Services.Presentation
{
	public interface ILoadEdgesRepository
	{
		IEnumerable<Edge> LoadGraph();
	}
}
