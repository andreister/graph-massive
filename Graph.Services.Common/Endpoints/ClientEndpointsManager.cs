using System;
using System.ServiceModel;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;

namespace Graph.Services.Common.Endpoints
{
	public class ClientEndpointsManager : IClientEndpointsManager
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		
		public TEndpoint GetService<TEndpoint>()
			where TEndpoint : IWcfEndpoint
		{
			var configuration = new EndpointConfiguration(typeof (TEndpoint));

			var uri = configuration.GetUri();
			try
			{
				var factory = new ChannelFactory<TEndpoint>(configuration.GetBinding());
				return factory.CreateChannel(new EndpointAddress(uri));
			}
			catch (Exception ex)
			{
				_logger.Error("Cannot access WCF endpoint at " + uri, ex);
				return default(TEndpoint);
			}
		}
	}
}
