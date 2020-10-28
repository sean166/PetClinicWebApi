using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }

        public string PetName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }
}
