using System.ServiceModel;

namespace Graph.Services.Common.Definitions
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface ITraversalService : IWcfEndpoint
	{
		[OperationContract]
		int FindShortestPath(int fromId, int toId);
	}
}
