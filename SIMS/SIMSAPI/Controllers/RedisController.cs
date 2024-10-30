using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using BCrypt.Net;

namespace SIMSAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisController(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        //Registrierung
        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUserDto registerUserDto)
        {
            var db = _connectionMultiplexer.GetDatabase();

            // Prüfen, ob der Benutzername bereits existiert
            if (db.KeyExists($"user:{registerUserDto.Username}"))
            {
                return Conflict("Benutzername existiert bereits.");
            }

            // Passwort hashen
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);

            // Benutzername und gehashtes Passwort in Redis speichern
            var userHash = new HashEntry[] {
                new HashEntry("username", registerUserDto.Username),
                new HashEntry("password", hashedPassword)
            };

            db.HashSet($"user:{registerUserDto.Username}", userHash);

            return Ok("Registrierung erfolgreich!");
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUserDto loginUserDto)
        {
            var db = _connectionMultiplexer.GetDatabase();

            // Prüfen, ob der Benutzer existiert
            if (!db.KeyExists($"user:{loginUserDto.Username}"))
            {
                return NotFound("Benutzer nicht gefunden.");
            }

            // Gehashtes Passwort aus Redis abrufen
            var storedPasswordHash = db.HashGet($"user:{loginUserDto.Username}", "password");

            // Passwort vergleichen
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginUserDto.Password, storedPasswordHash);

            if (!isPasswordValid)
            {
                return Unauthorized("Ungültiges Passwort.");
            }

            return Ok("Login erfolgreich!");
        }

        [HttpGet("users")]
        public ActionResult<List<RegisterUserDto>> GetAllUsers()
        {
            var db = _connectionMultiplexer.GetDatabase();

            // Alle Schlüssel finden, die mit "user:" beginnen
            var server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
            var userKeys = server.Keys(pattern: "user:*");

            var users = new List<RegisterUserDto>();

            // Alle Benutzerdaten von Redis abrufen
            foreach (var key in userKeys)
            {
                var userHash = db.HashGetAll(key);
                if (userHash.Length > 0)
                {
                    var user = new RegisterUserDto
                    {
                        Username = userHash.FirstOrDefault(h => h.Name == "username").Value,
                        Password = userHash.FirstOrDefault(h => h.Name == "password").Value
                    };
                    users.Add(user);
                }
            }

            return Ok(users);
        }

        [HttpDelete("users/{username}")]
        public ActionResult DeleteUser(string username)
        {
            var db = _connectionMultiplexer.GetDatabase();

            // Prüfen, ob der Benutzer existiert
            if (!db.KeyExists($"user:{username}"))
            {
                return NotFound("Benutzer nicht gefunden.");
            }

            // Löschen des Benutzers
            bool deleted = db.KeyDelete($"user:{username}");

            if (deleted)
            {
                return Ok("Benutzer erfolgreich gelöscht.");
            }
            else
            {
                return StatusCode(500, "Fehler beim Löschen des Benutzers.");
            }
        }


    }

    public class LoginUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


}
