using System;
using System.IO;
using System.ServiceProcess;
using Graph.Core.Logging;
using Graph.Services.Calculation;
using Graph.Services.Common.Endpoints;
using Graph.Services.Presentation;
using Graph.Services.Storage;

namespace Graph.Services
{
	public class WindowsService : ServiceBase
	{
		private static readonly ILogger _logger = LogManager.GetLogger();
		private readonly IServerEndpointsManager _manager;

		public WindowsService() : this(null)
		{
			Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
			AppDomain.CurrentDomain.UnhandledException += HandleException;
		}

		public WindowsService(IServerEndpointsManager manager = null)
		{
			_manager = manager ?? new ServerEndpointsManager();
		}

		public static void Main(string[] args)
		{
			StartInteractive(new WindowsService(), args);
		}

		protected override void OnStart(string[] args)
		{
			_manager.OpenEndpoint<StorageService>();
			_manager.OpenEndpoint<PresentationService>();
			_manager.OpenEndpoint<CalculationService>();

			_logger.Info("Windows service started");
		}

		protected override void OnStop()
		{
			_manager.CloseEndpoints();
			_logger.Info("Windows service stopped");
		}

		private void HandleException(object sender, UnhandledExceptionEventArgs e)
		{
			_logger.Error("Unexpected exception: " + e.ExceptionObject);
		}
		
		protected static void StartInteractive(WindowsService service, string[] args)
		{
			if (Environment.UserInteractive)
			{
				try {
					service.OnStart(args);

					_logger.Warn("The service is running in interactive mode. Press any key to exit.");
					Console.ReadKey();
				}
				finally {
					service.OnStop();
				}
			}
			else
			{
				Run(service);
			}
		}
	}
}
