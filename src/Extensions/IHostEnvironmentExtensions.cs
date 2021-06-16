using System.Linq;
using Microsoft.Extensions.Hosting;

namespace Customers.Extensions
{
  /// <summary>
  /// Extends <see cref="IHostEnvironment" />
  /// </summary>
  public static class IHostEnvironmentExtensions
  {
    /// <summary>
    /// Checks if environment is local
    /// </summary>
    /// <param name="hostEnvironment"></param>
    public static bool IsLocal(this IHostEnvironment hostEnvironment)
    {
      return new string[] { "local", "Local" }.Contains(hostEnvironment.EnvironmentName);
    }
  }
}