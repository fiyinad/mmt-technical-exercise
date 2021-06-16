namespace Customers.Settings
{
  /// <summary>
  /// Models NoSql Db config
  /// </summary>
  public class DbSettings
  {
    /// <summary>
    /// Gets or sets the database name
    /// </summary>
    /// <value></value>
    public string Database { get; set; }

    /// <summary>
    /// Gets or sets the database connection string
    /// </summary>
    /// <value></value>
    public string ConnectionString { get; set; } =
      "Server=tcp:sse.database.windows.net,1433;Initial Catalog=SSE_Test;Persist Security Info=False;User ID=mmt-sse-test;Password=database-user-01;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

    /// <summary>
    /// Gets or sets the mongo server address
    /// </summary>
    /// <value></value>
    public string Server { get; set; }
  }
}