using Graph.Core.Database.Repositories;

namespace Graph.Services.Storage
{
	internal class SaveGraphRepository : Repository, ISaveGraphRepository
	{
		public void DeleteExistingGraph()
		{
			Execute("DELETE FROM Edges; DELETE FROM Nodes");
		}
	}
}