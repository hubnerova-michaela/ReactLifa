using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthApp.Data;
using OAuthApp.Models;
using OAuthApp.Models.Inputs;
using System.Security.Claims;

namespace OAuthApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthorizationService _authorizationService;

    public UserController(ApplicationDbContext context, IAuthorizationService authorizationService)
    {
        _context = context;
        _authorizationService = authorizationService;
    }


    // POST: api/User
    [Authorize]
    [HttpPost("User")]
    public async Task<ActionResult<User>> NewUser(UserInput request)
    {
        var id = GetId();
        if (_context.Users.SingleOrDefault(x => x.Id == GetId()) == null)
        {
            User user = new User
            {
                Id = GetId(),
                Name = request.Name,
                Email = request.Email,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        return Ok();

    }

    private string GetId()
    {
        return User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;
    }
}

