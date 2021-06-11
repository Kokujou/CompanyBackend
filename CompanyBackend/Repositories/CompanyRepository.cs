using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyBackend.Models;
using CompanyBackend.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace CompanyBackend.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly Container _databaseContainer;

        public CompanyRepository(ICosmosDbDatabaseConnection connection)
        {
            _databaseContainer = connection.Database.GetContainer("CompanyBackend");
        }

        public async Task<IEnumerable<CompanyModel>> GetCompanies()
        {
            return await _databaseContainer.GetItemLinqQueryable<CompanyModel>().ToFeedIterator().ReadNextAsync();
        }

        public async Task<CompanyModel> GetCompany(Guid id)
        {
            return await _databaseContainer.ReadItemAsync<CompanyModel>(id.ToString(),
                new PartitionKey(nameof(CompanyModel)));
        }

        public async Task CreateCompany(CompanyModel company)
        {
            await _databaseContainer.CreateItemAsync(company, new PartitionKey(nameof(CompanyModel)),
                GetRequestOptions(company));
        }

        public async Task UpdateCompany(CompanyModel company)
        {
            await _databaseContainer.UpsertItemAsync(company, new PartitionKey(nameof(CompanyModel)),
                GetRequestOptions(company));
        }

        public async Task DeleteCompany(Guid id)
        {
            await _databaseContainer.DeleteItemAsync<CompanyModel>(id.ToString(),
                new PartitionKey(nameof(CompanyModel)));
        }

        private static ItemRequestOptions GetRequestOptions(CompanyModel model)
        {
            return new() {IfMatchEtag = model.ETag};
        }
    }
}