using admission_task.Common;
using admission_task.Dtos;
using admission_task.Exceptions;
using admission_task.Models;
using admission_task.Repos.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace admission_task.Repos
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthRepository(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(LoginDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null) throw new NotFoundException($"invalid username");
            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword) throw new NotFoundException($"invalid password"); 

            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", user.Id),
                    new Claim("email", user.Email ?? ""),
                    new Claim("fullName", user.Name ?? ""),
                    new Claim("role", JsonConvert.SerializeObject(userRoles.Select(s => s))),

                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        authClaims.Add(roleClaim);
                    }
                }
            }


            var token = GetToken(authClaims);

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

        }

        public async Task<Response> RegisterAsync(RegisterDto request)
        {
            var userExists = await _userManager.FindByNameAsync(request.Username);
            if (userExists != null) throw new admission_task.Exceptions.ApplicationException(" username is exist ");

            User user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username,
                Name = request.Name
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new admission_task.Exceptions.ApplicationException(" User creation failed! Please , try again ",result.Errors.Select(d=>d.Description).ToList());
            foreach (var role in request.Roles)
            {

                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
                if (await _roleManager.RoleExistsAsync(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }
            return new Response
            {
                Status = "success",
                Message = "user created successfully"
            };

        }
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
