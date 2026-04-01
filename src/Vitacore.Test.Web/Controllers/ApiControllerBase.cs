using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace Vitacore.Test.Web.Controllers
{
    /// <summary>
    /// Базовый API-контроллер
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, type: typeof(ProblemDetails))]
    public class ApiControllerBase : ControllerBase
    {
        protected Guid GetCurrentUserId()
        {
            var userIdValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                throw new UnauthorizedAccessException("Не удалось определить текущего пользователя.");
            }

            return userId;
        }
    }
}
