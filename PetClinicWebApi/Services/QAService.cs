using MongoDB.Driver;
using PetClinicWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Services
{
    public class QAService
    {
        private readonly IMongoCollection<QA> _qA;
        public QAService(IPetClinicDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _qA = database.GetCollection<QA>("QAs");
        }
        public async Task<List<QA>> GetAll()
        {
            return await _qA.Find(q => true).ToListAsync();
        }
        public async Task<QA> CreateAsync(QA qA)
        {
            await _qA.InsertOneAsync(qA);
            return qA;
        }
        public async Task UpdateAsync(string id, QA qA)
        {
            await _qA.ReplaceOneAsync(s => s.QAId == id, qA);
        }
        public async Task DeleteAsync(string id)
        {
            await _qA.DeleteOneAsync(s => s.QAId == id);
        }
    }
}
