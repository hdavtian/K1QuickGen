using K1QuickGen.Api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Repositories
{
    public class Form1065Repository
    {
        private readonly IMongoCollection<Form1065> _collection;

        public Form1065Repository(IMongoDatabase db)
        {
            _collection = db.GetCollection<Form1065>("Form1065s");
        }

        public Task InsertAsync(Form1065 form) =>
            _collection.InsertOneAsync(form);

        public Task<List<Form1065>> GetByCompanyIdAsync(Guid companyId) =>
            _collection.Find(x => x.CompanyId == companyId).ToListAsync();
    }
}
