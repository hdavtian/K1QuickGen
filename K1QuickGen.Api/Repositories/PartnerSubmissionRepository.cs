using K1QuickGen.Api.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K1QuickGen.Api.Repositories
{
    /// <summary>
    /// Repository for interacting with the PartnerSubmissions collection in MongoDB.
    /// Handles basic CRUD operations.
    /// </summary>
    public class PartnerSubmissionRepository
    {
        private readonly IMongoCollection<PartnerSubmission> _collection;

        /// <summary>
        /// Initializes a new instance of the repository and sets up the MongoDB collection reference.
        /// </summary>
        /// <param name="database">Injected IMongoDatabase instance from DI container.</param>
        public PartnerSubmissionRepository(IMongoDatabase database)
        {
            // Get the PartnerSubmissions collection from the database
            _collection = database.GetCollection<PartnerSubmission>("PartnerSubmissions");
        }

        /// <summary>
        /// Inserts a new partner submission into the collection.
        /// </summary>
        /// <param name="submission">The partner submission to insert.</param>
        /// <returns>Asynchronous Task representing the operation.</returns>
        public Task InsertAsync(PartnerSubmission submission)
        {
            return _collection.InsertOneAsync(submission);
        }

        /// <summary>
        /// Retrieves all partner submissions from the collection.
        /// </summary>
        /// <returns>A list of all PartnerSubmission documents.</returns>
        public Task<List<PartnerSubmission>> GetAllAsync()
        {
            return _collection.Find(_ => true).ToListAsync();
        }

        public Task<List<PartnerSubmission>> GetByForm1065IdAsync(Guid form1065Id) =>
            _collection.Find(x => x.Form1065Id == form1065Id).ToListAsync();
    }
}
