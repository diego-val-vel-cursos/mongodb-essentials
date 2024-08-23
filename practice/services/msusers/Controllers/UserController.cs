using Microsoft.AspNetCore.Mvc;
using Practice.Services.msusers.Models;
using Practice.Services.msusers.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice.Services.msusers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userService.GetAsync();
            return Ok(users);
        }

        // GET: api/User/:id
        [HttpGet("{id:length(24)}", Name = "GetUserById")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            await _userService.CreateAsync(user);
            return CreatedAtRoute("GetUserById", new { id = user.Id.ToString() }, user);
        }

        // PUT: api/User/:id
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateUser(string id, User updatedUser)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            updatedUser.Id = user.Id;
            await _userService.UpdateAsync(id, updatedUser);

            return NoContent();
        }

        // DELETE: api/User/:id
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userService.GetAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userService.RemoveAsync(id);

            return NoContent();
        }

        // POST: api/User/Login
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.AuthenticateAsync(loginRequest.Email, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(user);
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
