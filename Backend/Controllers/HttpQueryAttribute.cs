using Microsoft.AspNetCore.Mvc.Routing;

namespace MTGCC.Controllers
{
    public class HttpQueryAttribute : HttpMethodAttribute
    {
        private static readonly IEnumerable<string> _supportedMethods = new[] { "QUERY" };

        /// <summary>
        /// Creates a new <see cref="HttpQueryAttribute"/>.
        /// </summary>
        public HttpQueryAttribute()
            : base(_supportedMethods)
        {
        }

        /// <summary>
        /// Creates a new <see cref="HttpQueryAttribute"/> with the given route template.
        /// </summary>
        /// <param name="template">The route template. May not be null.</param>
        public HttpQueryAttribute(string template)
            : base(_supportedMethods, template)
        {
            if (template == null)
            {
                throw new ArgumentNullException(nameof(template));
            }
        }
    }
}
