using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rolebased_Authorization.Repository.Helpers.Interfaces;
using Rolebased_Authorization.Repository.Models;
using WebApi.Services;

namespace Rolebased_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserService _service;
        public AuthController(UserService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User userForLoginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userFromRepo = _service.Authenticate(userForLoginDto.Username, userForLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();

            return Ok(new { userFromRepo });

        }
    }
}