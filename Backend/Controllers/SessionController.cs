using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTGCC.Database;
using MTGCC.Services;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private AppUser ActiveUser => (AppUser)HttpContext.Items["Account"];

        private readonly AppDbContext _dbContext;
        private readonly TokenService _tokenService;

        public SessionController(AppDbContext dbContext, TokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register([FromBody] RegisterModel register)
        {
            if (register.Username == null)
                return BadRequest("Username parameter cannot be null!");
            if (register.Username.Length < 3)
                return BadRequest("Username parameter cannot be less than 3 characters!");
            if (register.Username.Length > 50)
                return BadRequest("Username parameter cannot be more than 50 characters!");

            if (register.Password == null)
                return BadRequest("Password parameter cannot be null!");
            if (register.Password.Length < 8)
                return BadRequest("Password parameter cannot be less than 8 characters!");

            if (await DoesUserExist(this._dbContext, register.Username))
                return BadRequest("Username already taken!");

            AppUser newUser = new AppUser(register.Username, register.Password);

            this._dbContext.Users.Add(newUser);
            await this._dbContext.SaveChangesAsync();

            return new UserModel()
            {
                Username = register.Username,
                Token = this._tokenService.CreateToken(newUser)
            };

            async Task<bool> DoesUserExist(AppDbContext dbContext, string username) { return await dbContext.Users.AnyAsync(x => x.Username.ToLower() == username.ToLower()); }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login([FromBody] LoginModel login)
        {
            if (login.Username == null)
                return BadRequest("Username parameter cannot be null!");
            if (login.Password == null)
                return BadRequest("Password parameter cannot be null!");

            var user = await this._dbContext.Users.SingleOrDefaultAsync(x => x.Username.ToLower() == login.Username.ToLower());

            if (user == null)
                return BadRequest("No account registered with the provided username.");

            if (!ValidatePassword(login.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized("Password was incorrect.");

            return new UserModel()
            {
                Username = login.Username,
                Token = this._tokenService.CreateToken(user)
            };

            bool ValidatePassword(string password, byte[] checkAgainst, byte[] salt)
            {
                var hmac = new HMACSHA512(salt);

                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != checkAgainst[i]) return false;
                }
                return true;
            }
        }

        [HttpPost("logout")]
        [AuthorizeSession]
        public async Task<ActionResult> Logout()
        {
            ActiveUser.SessionToken = "";
            this._dbContext.Update(ActiveUser);
            await this._dbContext.SaveChangesAsync();
            return Ok();
        }

        public class RegisterModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class UserModel
        {
            public string Username { get; set; }
            public string Token { get; set; }
        }

    }
}
