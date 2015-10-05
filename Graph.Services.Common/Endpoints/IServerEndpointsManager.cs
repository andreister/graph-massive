using Graph.Services.Common.Definitions;

namespace Graph.Services.Common.Endpoints
{
	public interface IServerEndpointsManager
	{
		void OpenEndpoint<TEndpoint>()
			where TEndpoint : IWcfEndpoint;

		void CloseEndpoints();
	}
}