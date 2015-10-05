using Graph.Core.Database.Repositories;

namespace Graph.Services.Storage
{
	public interface IStorageRepository : IRepository
	{
		void DeleteExistingGraph();
	}
}
