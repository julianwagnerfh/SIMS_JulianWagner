using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SIMSAPI.Models;
using StackExchange.Redis;

namespace SIMSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLoginController : ControllerBase
    {
        private readonly RedisService _redisService;

        public UserLoginController(RedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var db = _redisService.GetDatabase();

            // Überprüfen, ob der Benutzer bereits existiert
            var existingUser = await db.StringGetAsync(user.Username);
            if (!existingUser.IsNull)
            {
                return BadRequest("Benutzer existiert bereits.");
            }

            // Passwort hashen und speichern
            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            await db.StringSetAsync(user.Username, user.PasswordHash);

            return Ok("Benutzer registriert.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var db = _redisService.GetDatabase();

            // Überprüfen, ob der Benutzer existiert
            var storedHash = await db.StringGetAsync(user.Username);
            if (storedHash.IsNull)
            {
                return Unauthorized("Ungültiger Benutzername oder Passwort.");
            }

            // Passwort vergleichen
            var inputHash = PasswordHasher.HashPassword(user.PasswordHash);
            if (inputHash != storedHash)
            {
                return Unauthorized("Ungültiger Benutzername oder Passwort.");
            }

            // Hier könnte ein JWT-Token generiert werden (optional)
            return Ok("Login erfolgreich.");
        }
    }
}
