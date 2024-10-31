using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using PostService;

namespace PostIntegrationTests
{
    class MockFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new InMemoryDatabaseRoot();

            // builder.ConfigureServices(services =>
            // {
            //     services.RemoveAll(typeof(DbContextOptions<APIContext>));
            //     services.AddDbContext<APIContext>(options =>
            //         options.UseInMemoryDatabase("Testing", root));
            // });

            return base.CreateHost(builder);
        }
    }
}
