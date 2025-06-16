using Microsoft.AspNetCore.Http;

namespace Services.Shared.Context
{
    public interface IUserContext
    {
        (string Value, bool Exist) UserId { get; }
        (string Value, bool Exist) UserType { get; }
        HttpContext HttpContext { get; }
    }
}
