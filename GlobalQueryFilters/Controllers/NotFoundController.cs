using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GlobalQueryFilters.Controllers
{
    public class NotFoundController : Controller
    {
        [Route("{*url}", Order = 999)]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> FileNotFound()
        {
            // Default to returning the index page, for single-page app purposes
            // We could use UseStatusCodePagesWithReExecute, but this would screw up actual error codes in production
            // and make error handling on the client difficult because it catches all errors, not just 404s
            return File("~/index.html", "text/html");
        }
    }
}