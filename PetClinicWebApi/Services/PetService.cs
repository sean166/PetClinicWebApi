using MongoDB.Driver;
using PetClinicWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Services
{
    public class PetService
    {
        private readonly IMongoCollection<Pet> _pet;

        public PetService(IPetClinicDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _pet = database.GetCollection<Pet>(settings.PetCollectionName);
        }
        public List<Pet> Get() =>
            _pet.Find(pet => true).ToList();
        public Pet Get(string id) =>
            _pet.Find<Pet>(pet => pet.PetId == id).FirstOrDefault();

        public Pet Create(Pet pet)
        {
            _pet.InsertOne(pet);
            return pet;
        }

        public void Update(string id, Pet pet) =>
            _pet.ReplaceOne(p => p.PetId == id, pet);

        public void Remove(Pet pet) =>
            _pet.DeleteOne(p => p.PetId == pet.PetId);

        public void Remove(string id) =>
            _pet.DeleteOne(p => p.PetId == id);
    }
}
