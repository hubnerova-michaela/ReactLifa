using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuthApp.Data;
using OAuthApp.Models;
using OAuthApp.Models.Inputs;
using System.Security.Claims;

namespace OAuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthorizationService _authorizationService;
        public LocationController(ApplicationDbContext db, IAuthorizationService authorizationService)
        {
            _db = db;
            _authorizationService = authorizationService;
        }

        // GET: api/<LocationController>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_db.Locations);
        }

        // GET api/<LocationController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            if (_db.Locations.Any(l => l.LocationId == id))
            {
                return Ok(_db.Locations.Find(id));
            }
            else
            {
                return NotFound();
            };
        }

        [Authorize]
        [HttpDelete("Location/{id}")]
        public async Task<ActionResult<Location>> DeleteLocation(Guid id)
        {
            var UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var isAdmin = await _authorizationService.AuthorizeAsync(User, "Administrator");
            Location location = _db.Locations.FirstOrDefault(l => l.LocationId == id);
            if (location == null) return NotFound();
            Reality reality = _db.Realities.FirstOrDefault(r => r.RealityId == location.RealityId);
            if (reality == null) return NotFound();

            if (reality.UserId != UserId || !isAdmin.Succeeded)
            {
                return Unauthorized();
            }

            _db.Locations.Remove(location);
            await _db.SaveChangesAsync();
            return Ok(location);
        }



        // POST api/<CharacterController>
        [HttpPost("Location")]
        public async Task<ActionResult<Location>> NewLocation([FromBody] NewLocationInput newLocationInput)
        {
            if (_db.Realities.Any(x => x.ImgPath == newLocationInput.ImgPath))
            {
                return BadRequest();
            }
            Location location = new Location
            {
                LocationName = newLocationInput.LocationName,
                Description = newLocationInput.Description,
                ImagePath = newLocationInput.ImgPath,
                RealityId = newLocationInput.RealityId
            };

            _db.Locations.Add(location);
            await _db.SaveChangesAsync();
            return Ok(location);
        }


        // PUT api/<LocationController>/5
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] string value)
        //{
        //}

    }
}
