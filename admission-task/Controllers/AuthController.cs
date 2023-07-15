using admission_task.Common;
using admission_task.Dtos;
using admission_task.Models;
using admission_task.Repos.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace admission_task.Controllers
{
        [Route("api/[controller]")]
        [ApiController]
        public class AuthController : ControllerBase
        {
        private readonly IAuthRepository _authRepository;


        public AuthController(
              IAuthRepository authRepository)
            {
                _authRepository = authRepository;
            }

            [HttpPost]
            [Route("login")]
            public async Task<IActionResult> Login([FromBody] LoginDto model)
            {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
               var res= await _authRepository.LoginAsync(model);
                return Ok(res);
             
            }

            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register([FromBody] RegisterDto model)
            {
                if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
                var res=await _authRepository.RegisterAsync(model);
            return Ok(res);
        }

        }
    }

