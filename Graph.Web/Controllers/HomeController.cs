using System;
using System.Web.Mvc;
using Graph.Services.Common.Definitions;
using Graph.Services.Common.Endpoints;
using Newtonsoft.Json;

namespace Graph.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		
		[HttpPost]
		public JsonResult GetNodes()
		{
			var wcfManager = new ClientEndpointsManager();
			var service = wcfManager.GetService<IPresentationService>();
			var graph = service.GetGraph();

			var json = JsonConvert.SerializeObject(graph);
			return Json(json);
		}

		[HttpPost]
		public JsonResult FindPath(int from, int to)
		{
			throw new NotImplementedException();
		}
	}
}
