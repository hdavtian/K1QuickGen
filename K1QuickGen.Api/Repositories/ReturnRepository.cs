using K1QuickGen.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Repositories
{
    public class ReturnRepository
    {
        private readonly IMongoCollection<PartnershipReturn> _collection;

        public ReturnRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<PartnershipReturn>("PartnershipReturns");
        }

        public Task<List<PartnershipReturn>> GetByCompanyIdAsync(string companyId) =>
            _collection.Find(x => x.CompanyId == companyId).ToListAsync();

        public Task<PartnershipReturn?> GetByIdAsync(Guid id) =>
            _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public Task InsertAsync(PartnershipReturn returnDoc) =>
            _collection.InsertOneAsync(returnDoc);
    }
}
