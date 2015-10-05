using Graph.Core.Database.Repositories;

namespace Graph.Services.Storage
{
	internal class StorageRepository : Repository, IStorageRepository
	{
		public void DeleteExistingGraph()
		{
			Execute("DELETE FROM Edges; DELETE FROM Nodes");
		}
	}
}