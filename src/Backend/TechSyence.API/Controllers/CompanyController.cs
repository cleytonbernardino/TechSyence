using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSyence.Application.UseCases.Company.ListUsers;
using TechSyence.Communication;

namespace TechSyence.API.Controllers;

[Authorize(Roles = "ADMIN,RH,SUB_MANAGER,MANAGER")]
public class CompanyController : TechSyenceBaseController
{
    [HttpGet("Users")]
    [ProducesResponseType(typeof(ResponseListCompanyUser), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListUsers(
        [FromServices] IListCompanyUsersUseCase useCase
        )
    {
        var response = await useCase.Execute();
        return Ok(response);
    }
}
