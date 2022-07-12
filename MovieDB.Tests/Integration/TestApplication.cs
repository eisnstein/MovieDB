using Microsoft.AspNetCore.Mvc.Testing;

namespace MovieDB.Tests.Integration;

public class TestApplication : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test");
        base.ConfigureWebHost(builder);
    }
}
