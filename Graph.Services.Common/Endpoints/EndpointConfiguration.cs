using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Graph.Services.Common.Endpoints
{
	/// <summary>
	/// Abstraction for both service and client (proxy) endpoints. My goal is to avoid any XML configuration.
	/// </summary>
	public class EndpointConfiguration
	{
		private readonly Type _interfaceType;
		private static readonly TimeSpan _sendTimeout = TimeSpan.FromMinutes(1);
		private static readonly TimeSpan _receiveTimeout = TimeSpan.FromMinutes(3);
		private static readonly int _maxMessageSize = 2147483647; //2GB

		private const string ServiceUrlTemplate = "net.tcp://{0}:{1}/{2}";

		protected string Computer { get; set; }
		protected int Port { get; set; }

		public EndpointConfiguration(Type interfaceType)
		{
			if (!interfaceType.Name.StartsWith("I") || !interfaceType.IsInterface)
			{
				throw new Exception("Convention over configuration: all endpoints should have interfaces starting with 'I' and endpoint names will be just the interface names without the 'I'");
			}

			Computer = "localhost";
			Port = 9999;
			
			_interfaceType = interfaceType;
		}

		public Binding GetBinding()
		{
			return new NetTcpBinding(SecurityMode.None)
			{
				MaxReceivedMessageSize = _maxMessageSize,
				SendTimeout = _sendTimeout,
				ReceiveTimeout = _receiveTimeout
			};
		}

		public Uri GetUri()
		{
			var endpoint = _interfaceType.Name.Substring(1);
			return new Uri(string.Format(ServiceUrlTemplate, Computer, Port, endpoint));
		}
	}
}
