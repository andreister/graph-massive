using System.ServiceModel;
using System.Xml.Linq;

namespace Graph.Services.Common.Definitions
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IStorageService : IWcfEndpoint
	{
		[OperationContract]
		void SaveGraph(XElement nodes);
	}
}
