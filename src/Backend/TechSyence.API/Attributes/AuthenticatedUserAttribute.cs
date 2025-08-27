using Microsoft.AspNetCore.Mvc;
using TechSyence.API.Filter;

namespace TechSyence.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedUserAttribute() : TypeFilterAttribute(typeof(AuthenticatedUserFilter))
{
}
