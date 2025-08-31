using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSyence.API.Binders;
using TechSyence.Application.UseCases.User.Delete;
using TechSyence.Application.UseCases.User.Register;
using TechSyence.Application.UseCases.User.Update;
using TechSyence.Application.UseCases.User.Update.Password;
using TechSyence.Communication;

namespace TechSyence.API.Controllers;

public class UserController : TechSyenceBaseController
{
    [HttpPost]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterUser request,
        [FromServices] IRegisterUserUseCase useCase
        )
    {
        var response = await useCase.Execute(request);
        return Created("", response);
    }

    [HttpPut]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        [FromBody] RequestUpdateUser request,
        [FromServices] IUpdateUserUseCase useCase
        )
    {
        await useCase.Execute(request);
        return NoContent();
    }

    [HttpPut("change-password")]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdatePassword(
        [FromBody] RequestUpdateUserPassword request,
        [FromServices] IUpdateUserPasswordUseCase useCase
        )
    {
        await useCase.Execute(request);
        return NoContent();
    }
    
    [HttpDelete]
    [Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute] [ModelBinder(typeof(TechSyenceIdBinder))] long id,
        [FromServices] IDeleteUserUseCase useCase
        )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
