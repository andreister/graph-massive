using System.Collections.Generic;
using System.ServiceModel;

namespace Graph.Services.Common.Definitions
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	[ServiceKnownType(typeof(Edge))]
	public interface IPresentationService : IWcfEndpoint
	{
		[OperationContract]
		IEnumerable<Edge> GetGraph();
	}
}
