using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using services.Errors;

namespace services.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly StoreContext _context;
        public BuggyController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet("notFound")]
        public ActionResult GetNotFoundRequest()
        {
            var prod = _context.Products.Find(42);
            if (prod == null) 
            {
                return NotFound(new ServiceResponse(404));
            }
            return Ok();
        }

        [HttpGet("serverError")]
        public ActionResult GetServerError()
        {
            var prod = _context.Products.Find(42);
            var err = prod.ToString();
            return Ok();
        }

        [HttpGet("badRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ServiceResponse(400));
        }

        [HttpGet("badRequest/{id}")]
        public ActionResult GetValidationError(int id)
        {
            return Ok();
        }

        [HttpGet("testAuth")]
        [Authorize]
        public ActionResult<string> GetSecretTest() 
        {
            return "apzap dza secret";
        }

    }
}