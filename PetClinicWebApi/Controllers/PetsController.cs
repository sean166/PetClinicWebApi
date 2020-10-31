using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using PetClinicWebApi.Model;
using PetClinicWebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PetClinicWebApi.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly PetService _petService;
        public PetsController(PetService petService)
        {
            _petService = petService;

        }
        [HttpGet]
        public ActionResult<List<Pet>> Get() =>
            _petService.Get();
        
        // GET api/<PetsController>/5
        [HttpGet("{id:length(24)}", Name="GetPet")]
        public ActionResult<Pet> Get(string id)
        {
            var pet = _petService.Get(id);
            if(pet == null)
            {
                return NotFound();
            }
            return pet;
        }

        // POST api/<PetsController>
        [HttpPost]
        public ActionResult<Pet> Create(Pet pet)
        {
            _petService.Create(pet);
            return CreatedAtRoute("GetPet", new { id = pet.PetId.ToString() }, pet);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Pet pet)
        {
            var p = _petService.Get(id);

            if (p == null)
            {
                return NotFound();
            }

            _petService.Update(id, pet);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var p = _petService.Get(id);

            if (p == null)
            {
                return NotFound();
            }

            _petService.Remove(p.PetId);

            return NoContent();
        }
    }
}
