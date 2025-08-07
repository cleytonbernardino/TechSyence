using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Communiction.Requests;
using TechSyence.Communiction.Responses;

namespace TechSyence.API.Controllers;

public class UserController : TechSyenceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResgisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUserJson request,
        [FromServices] IRegisterUserUseCase useCase
        )
    {
        ResponseResgisteredUserJson response = await useCase.Execute(request);
        return Created("", response);
    }
}
