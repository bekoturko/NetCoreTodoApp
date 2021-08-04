using Microsoft.Extensions.Logging;
using NetCoreTodoApp.Framework.Abstract.Services;
using System;

namespace NetCoreTodoApp.Framework.Services
{
	public class LoggingService : ILoggingService
	{
		#region constructor

		private readonly ILogger<LoggingService> logger;

		public LoggingService(ILogger<LoggingService> logger)
		{
			this.logger = logger;
		}

		#endregion constructor

		#region exception

		public void LogError(string methodName, string message, Exception ex)
		{
			var errorMessage = CreateMessage(methodName, message);

			logger.LogError(ex, errorMessage);
		}

		#endregion exception

		#region exception with message only

		public void LogError(string methodName, string message)
		{
			var errorMessage = CreateMessage(methodName, message);

			logger.LogError(errorMessage);
		}

		#endregion exception with message only

		#region warning

		public void LogWarn(string methodName, string message)
		{
			var warnMessage = CreateMessage(methodName, message);

			logger.LogWarning(warnMessage);
		}

		#endregion warning

		#region info

		public void LogInfo(string methodName, string message)
		{
			var infoMessage = CreateMessage(methodName, message);

			logger.LogInformation(infoMessage);
		}

		#endregion info

		#region debug

		public void LogDebug(string methodName, string message)
		{
			var debugMessage = CreateMessage(methodName, message);

			logger.LogDebug(debugMessage);
		}

		#endregion debug

		#region create message

		private static string CreateMessage(string methodName, string message)
		{
			return $"({methodName}) - {message}";
		}

		#endregion create message
	}
}