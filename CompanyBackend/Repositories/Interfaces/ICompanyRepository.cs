using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyBackend.Models;

namespace CompanyBackend.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<CompanyModel>> GetCompaniesAsync();
        public Task<CompanyModel> GetCompanyAsync(Guid id);
        public Task CreateCompanyAsync(CompanyModel company);
        public Task UpdateCompanyAsync(CompanyModel company);
        public Task DeleteCompanyAsync(Guid id);
    }
}