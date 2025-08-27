using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using TechSyence.Communication;
using TechSyence.Domain.Repositories.User;
using TechSyence.Domain.Security.Token;
using TechSyence.Exceptions;
using TechSyence.Exceptions.ExceptionsBase;

namespace TechSyence.API.Filter;

public class AuthenticatedUserFilter(
    IAccessTokenClaims accessTokenValidator,
    IUserReadOnlyRepository repository
    ) : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenClaims _accessTokenValidator = accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository = repository;

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            string token = TokenOnRequest(context)!;

            Guid userIndentifier = _accessTokenValidator.GetUserIndentifier();

            bool exist = await _repository.ExistActiveUserWithIndentifier(userIndentifier);

            if (!exist)
                throw new TechSyenceException(ResourceMessagesException.USER_DOES_NOT_HAVE_PERMISSION);
        } 
        catch (SecurityTokenExpiredException)
        {
            var responseError = new ResponseError().Errors;
            responseError.Add(ResourceMessagesException.EXPIRED_TOKEN);
            context.Result = new UnauthorizedObjectResult(responseError);
        } 
        catch (TechSyenceException ex)
        {
            var responseError = new ResponseError().Errors;
            responseError.Add(ex.Message);
            context.Result = new UnauthorizedObjectResult(responseError);
        } 
        catch (Exception)
        {
            var responseError = new ResponseError().Errors;
            responseError.Add(ResourceMessagesException.USER_DOES_NOT_HAVE_PERMISSION);
            context.Result = new UnauthorizedObjectResult(responseError);
        }
    }

    private static string? TokenOnRequest(AuthorizationFilterContext context)
    {
        string? authorization = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authorization))
            throw new TechSyenceException(ResourceMessagesException.NO_TOKEN);

        return authorization["Bearer".Length..].Trim();
    }
}
