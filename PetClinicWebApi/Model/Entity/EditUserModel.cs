using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model.Entity
{
    public class EditUserModel
    {
        [BsonId]
        public string UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PetName { get; set; }
        public int Age { get; set; }
        public long ContactPhone { get; set; }
    }
}
