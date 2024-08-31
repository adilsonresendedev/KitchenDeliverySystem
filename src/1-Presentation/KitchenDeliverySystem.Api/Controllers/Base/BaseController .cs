using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDeliverySystem.Api.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult HandleErrorOrResult<T>(ErrorOr<T> result)
        {
            if (result.IsError)
            {
                var error = result.Errors.FirstOrDefault();
                if (error != null)
                {
                    if (error.Type == ErrorType.NotFound)
                    {
                        return NotFound(error.Description);
                    }
                    else if (error.Type == ErrorType.Validation)
                    {
                        return BadRequest(error.Description);
                    }
                }
                return StatusCode(500, "An unexpected error occurred.");
            }

            return Ok(result.Value);
        }
    }
}
