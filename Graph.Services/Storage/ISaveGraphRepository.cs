using Graph.Core.Database.Repositories;

namespace Graph.Services.Storage
{
	public interface ISaveGraphRepository : IRepository
	{
		void DeleteExistingGraph();
	}
}
