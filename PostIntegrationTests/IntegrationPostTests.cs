using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PostIntegrationTests
{
    [TestClass]
    public class IntegrationPostTests
    {
        private readonly HttpClient _client;

        public IntegrationPostTests()
        {
            var factory = new MockFactory();
            using (factory.Services.CreateScope())
            {
                // var provider = scope.ServiceProvider;
                // using (var APIContext = provider.GetRequiredService<APIContext>())
                // {
                //     APIContext.Database.EnsureCreatedAsync();
                //
                //     APIContext.Users.AddAsync(new User { Username = "oggiVAdmin", Id = 1 });
                //     APIContext.Users.AddAsync(new User { Username = "oggiVUser", Id = 2 });
                //     APIContext.SaveChangesAsync();
                // }
                _client = factory.CreateClient();
            }
        }

        [TestMethod]
        public async Task GetSkeleton()
        {
            var response = await _client.GetAsync("/posts/skeleton");
            var value = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.IsTrue(value.Contains("This is the Post Service Skeleton endpoint."));
        }
    }
}