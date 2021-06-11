using System;
using System.Threading.Tasks;
using CompanyBackend.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;

namespace CompanyBackend.Repositories
{
    public sealed class CosmosDbDatabaseConnection : ICosmosDbDatabaseConnection, IDisposable

    {
        private readonly CosmosClient _client;
        private readonly string _databaseName;
        private readonly string _containerName;

        public Database Database { get; }
        public Container Container { get; }

        public CosmosDbDatabaseConnection(IConfiguration configuration)
        {
            var databaseUrl = new Uri(configuration.GetValue<string>("Cosmos:DatabaseUrl"));
            var databaseToken = configuration.GetValue<string>("Cosmos:DatabaseToken");
            _client = new CosmosClientBuilder(databaseUrl.AbsoluteUri,
                databaseToken).WithSerializerOptions(new CosmosSerializationOptions
                {PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase}).Build();
            _databaseName = configuration.GetValue<string>("Cosmos:DatabaseName");
            _containerName = configuration.GetValue<string>("Cosmos:ContainerName");
            Database = _client.GetDatabase(_databaseName);
            Container = Database.GetContainer(_containerName);
        }

        public async Task EnsureDatabaseExistsAsync()
        {
            await _client.CreateDatabaseIfNotExistsAsync(_databaseName);
        }

        public async Task EnsureContainerExistsAsync()
        {
            await Database.CreateContainerIfNotExistsAsync(_containerName, "/cosmosEntityName");
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}