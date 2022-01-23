using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthApp.Data;
using OAuthApp.Models;
using OAuthApp.Models.Inputs;
using System.Security.Claims;

namespace OAuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthorizationService _authorizationService;
        public CharacterController(ApplicationDbContext db, IAuthorizationService authorizationService)
        {
            _db = db;
            _authorizationService = authorizationService;
        }


        // GET: api/<CharacterController>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Characters);
        }

        // GET api/<CharacterController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (_db.Characters.Any(c => c.CharacterId == id))
            {
                return Ok(_db.Characters.Find(id));
            }
            else
            {
                return NotFound();
            };
        }

        // POST api/<CharacterController>
        [HttpPost("Character")]
        public async Task<ActionResult<Reality>> NewCharacter([FromBody] NewCharacterInput newCharacterInput)
        {
            if (_db.Realities.Any(x => x.ImgPath == newCharacterInput.ImgPath))
            {
                return BadRequest();
            }
            Character character = new Character
            {
                Name = newCharacterInput.Name,
                Surname = newCharacterInput.Surname,
                ImagePath = newCharacterInput.ImgPath,
                RealityId = newCharacterInput.RealityId
            };

            _db.Characters.Add(character);
            await _db.SaveChangesAsync();
            return Ok(character);
        }


        // PUT api/<CharacterController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] NewCharacterInput newCharacterInput)
        {
            Character character = _db.Characters.FirstOrDefault(ch => ch.CharacterId == id);
            Character newCharacter = new Character
            {
                Name = newCharacterInput.Name,
                Surname = newCharacterInput.Surname,
                ImagePath = newCharacterInput.ImgPath,
                Age = newCharacterInput.Age,
                Birthdate = newCharacterInput.Birthdate,

            };
            character.Name = newCharacterInput.Name;
            character.Surname = newCharacterInput.Surname;
            character.ImagePath = newCharacterInput.ImgPath;

            _db.Entry(character).State = EntityState.Modified;




            


        }

        [Authorize]
        [HttpDelete("Character/{id}")]
        public async Task<ActionResult<Character>> DeleteCharacter(Guid id)
        {
            var UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var isAdmin = await _authorizationService.AuthorizeAsync(User, "Administrator");
            Character character = _db.Characters.FirstOrDefault(ch => ch.CharacterId == id);
            if (character == null) return NotFound();
            Reality reality = _db.Realities.FirstOrDefault(r => r.RealityId == character.RealityId);
            if (reality == null) return NotFound();

            if (reality.UserId != UserId || !isAdmin.Succeeded)
            {
                return Unauthorized();
            }

            _db.Characters.Remove(character);


                //var file = _db.Pictures.SingleOrDefault(x => x.OriginalName == character.ImgPath);
                //string[] Files = Directory.GetFiles("Images");
                //foreach (string i in Files)
                //{
                //    if (i.ToUpper().Contains(file.Id.ToUpper()))
                //    {
                //        System.IO.File.Delete(i);
                //    }
                //}
                //_db.Remove(file);

                //_db.Characters.Remove(character);
                await _db.SaveChangesAsync();
                return Ok(character);
            }
        }
}
