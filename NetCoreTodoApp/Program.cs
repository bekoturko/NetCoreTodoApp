using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;

namespace NetCoreTodoApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var logger = NLogBuilder.ConfigureNLog($"Config/nlog.config").GetCurrentClassLogger();

			try
			{
				logger.Info(nameof(Main), "Uygulama ba�l�yor. Konfig�rasyonlar yap�l�yor...");

				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				logger.Error("Everything is something happened", ex);
			}
			finally
			{
				logger.Info(nameof(Main), "Uygulama ba�lad�.");

				LogManager.Shutdown();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
			.ConfigureLogging(logging =>
			{
				logging.ClearProviders();
				logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
			})
			.UseNLog()
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
	}
}