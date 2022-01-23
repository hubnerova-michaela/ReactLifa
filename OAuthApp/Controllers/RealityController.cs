using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OAuthApp.Data;
using OAuthApp.Models;
using OAuthApp.Models.Inputs;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OAuthApp.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RealityController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IAuthorizationService _authorizationService;
        public RealityController(ApplicationDbContext db, IAuthorizationService authorizationService)
        {
            _db = db;
            _authorizationService = authorizationService;
        }

        // GET: api/<RealityController>
        [Authorize]
        [HttpGet("Reality")]
        public async Task<ActionResult<IEnumerable<Reality>>> GetRealities()
        {
            var UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            IEnumerable<Reality> Realities = _db.Realities.Where(r => r.UserId == UserId).AsEnumerable();
            return Ok(Realities);
        }

        [Authorize]
        // GET api/<RealityController>/5
        [HttpGet("Reality/{id}")]
        public async Task<ActionResult<Reality>> GetReality(Guid id)
        {
            Reality reality = _db.Realities.Include(x => x.User).SingleOrDefault(x => x.RealityId == id);
            if (reality != null)
            {
                return Ok(reality);
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        // POST api/<RealityController>
        [HttpPost("Reality")]
        public async Task<ActionResult<Reality>> NewReality([FromBody] NewRealityInput newRealityInput)
        {
            if (_db.Realities.Any(x => x.ImgPath == newRealityInput.ImagePath))
            {
                return BadRequest();
            }
            Reality reality = new Reality
            {
                RealityName = newRealityInput.RealityName,
                GeneralInfo = newRealityInput.GeneralInfo,
                ImgPath = newRealityInput.ImagePath,
                UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value
            };

            _db.Realities.Add(reality);
            await _db.SaveChangesAsync();
            return Ok(reality);
        }

        [Authorize]
        [HttpDelete("Reality/{id}")]
        public async Task<ActionResult<Reality>> DeleteReality(Guid id)
        {
            var UserId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var isAdmin = await _authorizationService.AuthorizeAsync(User, "Administrator");
            Reality reality = _db.Realities.FirstOrDefault(r => r.RealityId == id);
            if (reality == null) return NotFound();

            if (reality.UserId != UserId || !isAdmin.Succeeded)
            {
                return Unauthorized();
            }

            _db.Realities.Remove(reality);
            await _db.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPatch("Reality/{id}")]
        public async Task<ActionResult<Reality>> EditReality (Guid id, [FromBody] JsonPatchDocument<Reality> patch)
        {
            var userId = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
            var isAdmin = await _authorizationService.AuthorizeAsync(User, "Administrator");
            Reality reality = _db.Realities.FirstOrDefault(r => r.RealityId == id);
            if (reality != null && (reality.UserId == userId || isAdmin.Succeeded))
            {
                patch.ApplyTo(reality, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _db.Entry(reality).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Ok(reality);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
