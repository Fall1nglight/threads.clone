using Threads.Api.Data.Users;

namespace Threads.Api.Features.Auth.Services.JwtProvider;

public interface IJwtProvider
{
    public Task<string> GenerateJwt(User user);
}
