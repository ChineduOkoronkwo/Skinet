using Microsoft.AspNetCore.Mvc;
using services.Errors;

namespace services.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code) {
            return new ObjectResult(new ServiceResponse(code));
        }
        
    }
}