using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiGestionEmpleados.Modelo;
using ApiGestionEmpleados.Modelo.Dtos;

namespace ApiGestionEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<Usuario> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(model.Email);
                if (userExists != null)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "El usuario ya existe"
                    });
                }

                var user = new Usuario
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    FechaCreacion = DateTime.Now,
                    Activo = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return Ok(new AuthResponseDto
                    {
                        Success = true,
                        Message = "Usuario registrado exitosamente"
                    });
                }

                return BadRequest(new AuthResponseDto
                {
                    Success = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(ClaimTypes.Name, user.Nombre),
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    };

                    var token = GetToken(authClaims);
                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new AuthResponseDto
                    {
                        Token = tokenString,
                        Expiration = token.ValidTo,
                        Email = user.Email!,
                        Nombre = user.Nombre,
                        Success = true,
                        Message = "Login exitoso"
                    });
                }

                return Unauthorized(new AuthResponseDto
                {
                    Success = false,
                    Message = "Email o contraseña incorrectos"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AuthResponseDto
                {
                    Success = false,
                    Message = $"Error interno del servidor: {ex.Message}"
                });
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}