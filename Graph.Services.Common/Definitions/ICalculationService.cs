using System.ServiceModel;

namespace Graph.Services.Common.Definitions
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface ICalculationService : IWcfEndpoint
	{
		[OperationContract]
		void FindShortestPath(int startNodeId, int endNodeId);
	}
}
