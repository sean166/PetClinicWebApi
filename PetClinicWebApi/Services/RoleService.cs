using MongoDB.Driver;
using PetClinicWebApi.Model;
using PetClinicWebApi.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Services
{
    public class RoleService
    {
        private readonly IMongoCollection<ApplicationRole> _role;

        public RoleService(IPetClinicDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _role = database.GetCollection < ApplicationRole>(settings.PetCollectionName);
        }
        public async Task<List<ApplicationRole>> GetAll()
        {
            return await _role.Find(s => true).ToListAsync();
        }
        public async Task<ApplicationRole> GetById(string id)
        {
            return await _role.Find(s => s.Id.ToString() == id ).FirstOrDefaultAsync();
        }


    }
}
