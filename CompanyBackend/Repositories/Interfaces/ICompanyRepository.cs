using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompanyBackend.Models;

namespace CompanyBackend.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        public Task<IEnumerable<CompanyModel>> GetCompanies();
        public Task<CompanyModel> GetCompany(Guid id);
        public Task CreateCompany(CompanyModel company);
        public Task UpdateCompany(CompanyModel company);
        public Task DeleteCompany(Guid id);
    }
}