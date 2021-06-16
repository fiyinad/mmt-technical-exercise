using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Customers
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

      var configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", false)
          .AddJsonFile($"appsettings.{environment}.json", true)
          .AddEnvironmentVariables()
          .Build();

      var host = WebHost.CreateDefaultBuilder(args)
          .UseConfiguration(configuration)
          .UseSerilog()
          .UseStartup<Startup>()
          .Build();

      Log.Logger = new LoggerConfiguration()
      .ReadFrom.Configuration(configuration)
      .CreateLogger();

      try
      {
        host.Run();
      }
      catch (Exception e)
      {
        Log.Error(e, string.Empty);
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }
  }
}
