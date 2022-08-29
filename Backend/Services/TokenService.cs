using Microsoft.IdentityModel.Tokens;
using MTGCC.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MTGCC.Services
{
    /// <summary>
    /// Service that creates and validates Json web tokens.
    /// </summary>
    public class TokenService
    {
        private readonly AppDbContext _context;
        private readonly SymmetricSecurityKey _key;

        /// <summary>
        /// Construct a new instance of TokenService.
        /// </summary>
        /// <param name="config">The application configuration.</param>
        /// <param name="context">The database context.</param>
        public TokenService(IConfiguration config, AppDbContext context)
        {
            _context = context;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenSecret"]));
        }

        /// <summary>
        /// Create a new Json token associated to the provided user.
        /// </summary>
        /// <param name="user">The user to associate the Json token with.</param>
        /// <returns>The newly created Json token.</returns>
        public string CreateToken(AppUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.ID) }),
                Expires = DateTime.UtcNow.AddYears(1),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string tokenStr = tokenHandler.WriteToken(token);
            user.SessionToken = tokenStr;
            _context.Update(user);
            _context.SaveChanges();

            return tokenStr;
        }

        /// <summary>
        /// Validate a Json token.
        /// </summary>
        /// <param name="token">The token to validate.</param>
        /// <param name="user">The user associated with the token.</param>
        /// <returns>True if the token is valid, otherwise false.</returns>
        public bool ValidateToken(string token, out AppUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
                string accountId = jwtToken.Claims.First(x => x.Type == "id").Value;

                user = _context.Users.Where(x => x.ID == accountId).FirstOrDefault();

                if (user.SessionToken != token)
                {
                    user = null;
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                user = null;
                return false;
            }
        }

    }
}
