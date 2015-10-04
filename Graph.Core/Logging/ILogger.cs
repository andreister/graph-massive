using System;

namespace Graph.Core.Logging
{
	/// <summary>
	/// Logger facade to abstract away the concrete logging framework we use.
	/// </summary>
	public interface ILogger
	{
		string Name { get; }
		bool IsEnabled(LogLevel level);

		void Debug(string message, Exception exception = null);
		void Info(string message, Exception exception = null);
		void Warn(string message, Exception exception = null);
		void Error(string message, Exception exception = null);
	}
}
