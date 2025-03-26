using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace PetNameGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PetNameController : ControllerBase
    {
        private static readonly string[] dogNames = new string[] { "Buddy", "Max", "Charlie", "Rocky", "Rex" };
        private static readonly string[] catNames = new string[] { "Whiskers", "Mittens", "Luna", "Simba", "Tiger" };
        private static readonly string[] birdNames = new string[] { "Tweety", "Sky", "Chirpy", "Raven", "Sunny" };

      

        public class PetNameRequest
        {
            public string AnimalType { get; set; }
            public bool? TwoPart { get; set; }
        }
        [HttpPost("generate")]

        public IActionResult GeneratePetName([FromBody] PetNameRequest request)
        {
            if (string.IsNullOrEmpty(request.AnimalType))
            {
                return BadRequest(new { error = "The 'animalType' field is required." });
            }

            string[] validAnimalTypes = { "dog", "cat", "bird" };
            if (!validAnimalTypes.Contains(request.AnimalType.ToLower()))
            {
                return BadRequest(new { error = "Invalid animal type. Allowed values: dog, cat, bird." });
            }

            if (request.TwoPart.HasValue && request.TwoPart != true && request.TwoPart != false)
            {
                return BadRequest(new { error = "The 'twoPart' field must be a boolean (true or false)." });
            }

            string[] selectedNames = request.AnimalType.ToLower() switch
            {
                "dog" => dogNames,
                "cat" => catNames,
                "bird" => birdNames,
                _ => throw new InvalidOperationException("Invalid animal type.")
            };

            Random rnd = new Random();
            string petName;

            if (request.TwoPart.GetValueOrDefault(false))
            {
                string firstName = selectedNames[rnd.Next(selectedNames.Length)];
                string secondName = selectedNames[rnd.Next(selectedNames.Length)];
                petName = firstName + secondName;
            }
            else
            {
                petName = selectedNames[rnd.Next(selectedNames.Length)];
            }

            return Ok(new { name = petName });
        }
    }


}
