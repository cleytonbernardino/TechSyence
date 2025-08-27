using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TechSyence.API.ControllersAdmin;

[Route("admin/[Controller]")]
[ApiController]
[Authorize(Roles = "ADMIN")]
public class TechSyenceAdminBaseController : ControllerBase
{
}
