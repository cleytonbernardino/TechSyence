using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.UseCases.Login;
using TechSyence.Communication;

namespace TechSyence.API.Controllers;

[Route("[Controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResgisteredUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RequestLogin), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLogin request
        )
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}
