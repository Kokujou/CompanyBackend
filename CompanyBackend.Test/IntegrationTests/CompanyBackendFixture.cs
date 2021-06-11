using System.Collections.Generic;
using CompanyBackend.Repositories.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CompanyBackend.Test.IntegrationTests
{
    [CollectionDefinition(nameof(CompanyBackendFixture))]
    public class InstallationLogicCollection : ICollectionFixture<CompanyBackendFixture>
    {
    }

    public class CompanyBackendFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, builder) =>
            {
                builder.AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Cosmos:ContainerName", "TestContainer"}
                });
            });
            base.ConfigureWebHost(builder);
        }

        protected override void Dispose(bool disposing)
        {
            RemoveTestDatabase();
            base.Dispose(disposing);
        }

        private void RemoveTestDatabase()
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ICosmosDbDatabaseConnection>();
            context?.Database.GetContainer("TestContainer").DeleteContainerAsync().Wait();
        }
    }
}