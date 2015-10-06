using System.Web.Mvc;
using Graph.Services.Common.Definitions;
using Graph.Services.Common.Endpoints;
using Newtonsoft.Json;

namespace Graph.Web.Controllers
{
	public class GraphController : Controller
	{
		[HttpPost]
		public JsonResult Edges()
		{
			var service = GetService<IPresentationService>();
			var graph = service.GetGraph();

			return Json(graph);
		}

		[HttpPost]
		public JsonResult Path(int from, int to)
		{
			var service = GetService<ITraversalService>();
			var path = service.FindShortestPath(from, to);

			var json = JsonConvert.SerializeObject(path);
			return Json(json);
		}

		private static TService GetService<TService>()
			where TService : IWcfEndpoint
		{
			var wcfManager = new ClientEndpointsManager();
			return wcfManager.GetService<TService>();
		}
	}
}
