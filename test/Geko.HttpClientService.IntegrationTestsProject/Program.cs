namespace Geko.HttpClientService.IntegrationTestsProject;

/// <summary>
/// ASP.NET Core web application is actually a console project.
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point of the project.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    /// <summary>
    /// Creates an <see cref="IHostBuilder"/>.
    /// </summary>
    /// <param name="args">The command line args.</param>
    /// <returns>A program initialization abstraction.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
.ConfigureWebHostDefaults(webBuilder =>
{
    webBuilder.UseStartup<Startup>();
});
    }
}
