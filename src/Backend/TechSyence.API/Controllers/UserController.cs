using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Communication;

namespace TechSyence.API.Controllers;

public class UserController : TechSyenceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResgisteredUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RequestLogin), StatusCodes.Status400BadRequest)]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUser request,
        [FromServices] IRegisterUserUseCase useCase
        )
    {
        ResponseResgisteredUser response = await useCase.Execute(request);
        return Created("", response);
    }
}
