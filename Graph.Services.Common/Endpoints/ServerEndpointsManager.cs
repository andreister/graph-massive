using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Graph.Core.Logging;
using Graph.Services.Common.Definitions;

namespace Graph.Services.Common.Endpoints
{
	public class ServerEndpointsManager : IServerEndpointsManager
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		private readonly List<ServiceHost> _hosts = new List<ServiceHost>();

		public void OpenEndpoint<TEndpoint>()
			where TEndpoint : IWcfEndpoint, new()
		{
			var endpointType = typeof (TEndpoint);
			var interfaces = endpointType.GetInterfaces().Where(x => x.Name != typeof(IWcfEndpoint).Name).ToList();
			if (interfaces.Count > 1) {
				throw new Exception("Convention over configuration: each endpoint should do one thing and implement just one interface");
			}
			var interfaceType = interfaces[0];

			var configuration = new EndpointConfiguration(interfaceType);
			var address = configuration.GetUri();

			try
			{
				var host = new ServiceHost(endpointType);
				host.AddServiceEndpoint(interfaceType, configuration.GetBinding(), address);
				host.Open();

				_hosts.Add(host);
				_logger.Info("WCF endpoint opened at " + address);
			}
			catch (Exception ex)
			{
				_logger.Error("Failed to open WCF endpoint at " + address, ex);
			}
		}

		public void CloseEndpoints()
		{
			foreach (var host in _hosts.Where(x => x != null && x.State == CommunicationState.Opened))
			{
				try
				{
					host.Close();
				}
				catch (Exception ex)
				{
					var endpoint = host.Description.Endpoints.Single();
					_logger.Error("Failed to close WCF endpoint " + endpoint.Address, ex);
				}
			}
		}
	}
}
