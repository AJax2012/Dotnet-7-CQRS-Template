using Meziantou.Extensions.Logging.Xunit;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using ExampleProject.Api;

using Xunit.Abstractions;

namespace ExampleProject.Integration.Test;

public class ExampleProjectApiFactory : WebApplicationFactory<IApiMarker>
{
    private ITestOutputHelper _testOutputHelper = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.Services.AddSingleton<ILoggerProvider>(_ => new XUnitLoggerProvider(_testOutputHelper));
        });
    }
}