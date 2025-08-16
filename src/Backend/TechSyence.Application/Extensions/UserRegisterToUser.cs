using TechSyence.Communication.Requests;
using TechSyence.Domain.Entities;

namespace TechSyence.Application.Extensions;

internal static class UserRegisterToUser
{
    public static User ToUser(this RequestRegisterUserJson request)
    {
        return new User()
        {
            Email = request.Email,
            Phone = request.Phone,
            FirstName = request.FirstName,
            LastName = request.LastName
        };
    }
}
