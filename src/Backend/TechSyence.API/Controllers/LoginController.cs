using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.UseCases.Login;
using TechSyence.Communication.Requests;
using TechSyence.Communication.Responses;

namespace TechSyence.API.Controllers;

public class LoginController : TechSyenceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResgisteredUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLoginJson request
        )
    {
        var response = await useCase.Execute(request);
        return Ok(response);
    }
}
