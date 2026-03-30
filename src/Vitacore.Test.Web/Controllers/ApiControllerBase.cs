using Microsoft.AspNetCore.Mvc;
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
    }
}