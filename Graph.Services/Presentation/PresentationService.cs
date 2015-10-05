using System.Collections.Generic;
using Graph.Services.Common.Definitions;

namespace Graph.Services.Presentation
{
	internal class PresentationService : IPresentationService
	{
		private readonly ILoadEdgesRepository _repository;

		public PresentationService()
		{
			_repository = new LoadEdgesRepository();
		}

		public PresentationService(ILoadEdgesRepository repository)
		{
			_repository = repository;
		}
		
		public IEnumerable<Edge> GetGraph()
		{
			return _repository.LoadGraph();
		}
	}
}
