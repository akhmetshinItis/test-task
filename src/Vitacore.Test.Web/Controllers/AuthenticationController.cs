using Vitacore.Test.Contracts.Requests.Auth.Login;
using Vitacore.Test.Contracts.Requests.Auth.Register;
using Vitacore.Test.Core.Authorization;
using Vitacore.Test.Core.Requests.Auth.Login;
using Vitacore.Test.Core.Requests.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        /// Регистрация пользователя
        /// </summary>
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(RegisterResponse))]
        [HttpPost("register")]
        public async Task<RegisterResponse> RegisterAsync(
            [FromServices] IMediator mediator,
            [FromBody] RegisterRequest request,
            CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            return await mediator.Send(new RegisterCommand(request), cancellationToken);
        }

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

        /// <summary>
        /// Информация о текущем пользователе
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            return Ok(new
            {
                userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
                userName = User.Identity?.Name,
                email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value,
                roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(x => x.Value).ToArray()
            });
        }

        /// <summary>
        /// Эндпоинт только для администраторов
        /// </summary>
        [Authorize(Roles = ApplicationRoles.Admin)]
        [HttpGet("admin")]
        public IActionResult AdminOnly()
        {
            return Ok(new
            {
                message = "Доступ разрешен только администраторам."
            });
        }
    }
}
