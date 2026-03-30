using Vitacore.Test.Contracts.Requests.Auth.Login;
using Vitacore.Test.Core.Requests.Auth.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using LoginRequest = Vitacore.Test.Contracts.Requests.Auth.Login.LoginRequest;

namespace Vitacore.Test.Web.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации
    /// </summary>
    public class AuthenticationController : ApiControllerBase
    {
        /// <summary>
        /// Логин
        /// </summary>
        /// <param name="mediator">Медиатор CQRS</param>
        /// <param name="request">Запрос на логин</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>Ответ на запрос на логин / JWT-токены</returns>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(LoginResponse))]
        [HttpPost("login")]
        public async Task<LoginResponse> LoginAsync(
            [FromServices] IMediator mediator,
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);
            
            return await mediator.Send(new LoginCommand(request), cancellationToken);
        }
    }
}