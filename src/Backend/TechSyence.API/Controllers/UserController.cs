using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Application.UseCases.User.Update;
using TechSyence.Communication;

namespace TechSyence.API.Controllers;

[Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
public class UserController : TechSyenceBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseResgisteredUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RequestLogin), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUser request,
        [FromServices] IRegisterUserUseCase useCase
        )
    {
        var response = await useCase.Execute(request);
        return Created("", response);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(RequestLogin), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromBody] RequestUpdateUser request,
        [FromServices] IUpdateUserUseCase useCase
        )
    {
        await useCase.Execute(request);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePassword(
        [FromBody] RequestUpdateUserPassword request,
        [FromServices] IUpdateUserUseCase useCase
        )
    {
        await useCase.Execute(request);
        return NoContent();
    }
}
