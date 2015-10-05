using System.ServiceModel;
using System.Xml.Linq;

namespace Graph.Services.Common.Definitions
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IPresentationService : IWcfEndpoint
	{
		[OperationContract]
		XElement GetGraph();
	}
}
