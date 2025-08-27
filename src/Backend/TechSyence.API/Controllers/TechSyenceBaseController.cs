using Microsoft.AspNetCore.Mvc;
using TechSyence.API.Attributes;

namespace TechSyence.API.Controllers;

[Route("[Controller]")]
[ApiController]
[AuthenticatedUser]
public class TechSyenceBaseController : ControllerBase
{
}
