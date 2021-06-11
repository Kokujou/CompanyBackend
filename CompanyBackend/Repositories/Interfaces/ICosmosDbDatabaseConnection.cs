using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace CompanyBackend.Repositories.Interfaces
{
    public interface ICosmosDbDatabaseConnection
    {
        Database Database { get; }
        Container Container { get; }
        Task EnsureDatabaseExistsAsync();
        Task EnsureContainerExistsAsync();
    }
}