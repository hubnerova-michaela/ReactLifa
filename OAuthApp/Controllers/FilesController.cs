using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OAuthApp.Models;
using OAuthApp.Services;

namespace OAuthApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileStorageManager _fsm;

        public FilesController(FileStorageManager fsm)
        {
            _fsm = fsm;
        }

        // GET: api/<FilesController>
        [HttpGet("files")]
        public async Task<IEnumerable<Picture>> Get()
        {
            return await _fsm.ListAsync();
        }

        // GET api/<FilesController>/5
        [HttpGet("files/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FilesController>
        [HttpPost("files")]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FilesController>/5
        [DisableRequestSizeLimit]
        [HttpPut("files/{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FilesController>/5
        [HttpDelete("files/{id}")]
        public void Delete(int id)
        {
        }
    }
}
