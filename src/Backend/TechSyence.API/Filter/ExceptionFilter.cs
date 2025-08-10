using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechSyence.Communiction.Responses;
using TechSyence.Exceptions.ExceptionsBase;
#if !DEBUG
using TechSyence.Exceptions;
#endif

namespace TechSyence.API.Filter;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is TechSyenceException)
        {
            HandleProjectException(context);
        }else
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(ExceptionContext context)
    {
        if (context.Exception is InvalidLoginException)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(context.Exception.Message));
        }
        else if (context.Exception is ErrorOnValidationException)
        {
            var exception = context.Exception as ErrorOnValidationException;
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(new ResponseErrorJson(exception!.ErrorMessages));
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
#if DEBUG
        context.Result = new ObjectResult(new ResponseErrorJson(context.Exception.Message));
#else
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
#endif
    }
}
