using AspNetCore.Identity.Mongo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model.Identity
{
    public class ApplicationUser:MongoUser
    {
       
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PetName { get; set; }
        public int Age { get; set; }
        

    }
}
