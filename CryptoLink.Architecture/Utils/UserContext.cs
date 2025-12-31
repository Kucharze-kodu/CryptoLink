using CryptoLink.Application.Utils;
using CryptoLink.Architecture.Utils.Extensions;
using Microsoft.AspNetCore.Http;

namespace CryptoLink.Architecture.Utils;

public sealed class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool? IsAuthenticated => _httpContextAccessor
        .HttpContext?
        .User
        .Identity?
        .IsAuthenticated;

    public int? UserId => _httpContextAccessor
        .HttpContext?
        .User
        .GetUserId();

    public string? UserName => _httpContextAccessor
        .HttpContext?
        .User
        .GetUserName();
}