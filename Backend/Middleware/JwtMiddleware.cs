using MTGCC.Database;
using MTGCC.Services;

namespace MTGCC.Middleware
{
    /// <summary>
    /// Json web Token Middleware that validates Auth Tokens and places the user into the current context.
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, TokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachAccountToContext(context, tokenService, token);

            await _next(context);
        }

        /// <summary>
        /// Validate the Auth Token and place the user into the current context.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="tokenService">The current instance of TokenService.</param>
        /// <param name="token">The token provided with the current request.</param>
        private void AttachAccountToContext(HttpContext context, TokenService tokenService, string token)
        {
            if (tokenService.ValidateToken(token, out AppUser user))
                context.Items["Account"] = user;
        }
    }
}
