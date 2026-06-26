using Microsoft.Extensions.Options;

namespace Threads.Api.Features.Auth.Services.RefreshToken;

public class RefreshTokenOptionsSetup : IConfigureOptions<RefreshTokenOptions>
{
    private readonly IConfiguration _configuration;

    public RefreshTokenOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(RefreshTokenOptions options)
    {
        _configuration.Bind(nameof(RefreshTokenOptions), options);
    }
}
