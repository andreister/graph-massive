using Graph.Services.Common.Definitions;

namespace Graph.Services.Common.Endpoints
{
	public interface IClientEndpointsManager
	{
		TEndpoint GetService<TEndpoint>()
			where TEndpoint : IWcfEndpoint;
	}
}
