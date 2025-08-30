using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TechSyence.Communication;
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
        var responseError = new ResponseError().Errors;
        switch (context.Exception)
        {
            case InvalidLoginException:
                responseError.Add(context.Exception.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(responseError);
                break;
            case NoPermissionException:
                responseError.Add(context.Exception.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Result = new UnauthorizedObjectResult(responseError);
                break;
            case ErrorOnValidationException exception:
                responseError.Add(exception.ErrorMessages);
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new BadRequestObjectResult(responseError);
                break;
            case NotFoundException notFoundException:
                responseError.Add(notFoundException.Message);
                context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Result = new NotFoundObjectResult(responseError);
                break;
        }
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        var responseError = new ResponseError().Errors;
#if DEBUG
        responseError.Add(context.Exception.Message);
        context.Result = new ObjectResult(responseError);
#else
        responseError.Add(ResourceMessagesException.UNKNOWN_ERROR);
        context.Result = new ObjectResult(new ResponseError());
#endif
    }
}
