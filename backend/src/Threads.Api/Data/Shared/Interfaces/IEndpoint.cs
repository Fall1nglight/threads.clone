namespace Threads.Api.Data.Shared.Interfaces;

public interface IEndpoint
{
    // abstract is needed because since C# 8 interfaces
    //  are allowed to have static methods
    static abstract void Map(IEndpointRouteBuilder builder);
}
