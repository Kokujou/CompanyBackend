using System;

namespace CompanyBackend.Models
{
    public class CompanyModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string LegalEntity { get; set; }
        public int Employees { get; set; }
        public int Equity { get; set; }

        public string CosmosEntityName => nameof(CompanyModel);
        public string ETag { get; set; }
    }
}