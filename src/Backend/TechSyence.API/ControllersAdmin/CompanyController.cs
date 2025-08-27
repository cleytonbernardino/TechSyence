using Microsoft.AspNetCore.Mvc;
using TechSyence.API.ControllersAdmin;
using TechSyence.Application.UseCases.Company.List;
using TechSyence.Application.UseCases.Company.Register;
using TechSyence.Communication;

namespace TechSyence.API.AdminControllers;

public class CompanyController : TechSyenceAdminBaseController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterCompany request,
        [FromServices] IRegisterCompanyUseCase useCase
        )
    {
        await useCase.Execute(request);
        return Created("", null);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseShortCompanies), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListCompanies(
        [FromServices] IListCompaniesUseCase useCase
        )
    {
        var response = useCase.Execute();
        return Ok(response);
    }
}
