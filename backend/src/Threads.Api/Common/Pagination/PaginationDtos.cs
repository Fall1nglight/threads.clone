namespace Threads.Api.Common.Pagination;

public record Cursor(DateTime LastDate, Guid LastId);

public record PagedRequest
{
    public string? Cursor { get; init; }
    public int PageSize
    {
        get;
        init => field = int.Clamp(value, 1, 100);
    } = 20;
};

public record PagedResponse<TResponseDto>(List<TResponseDto> Items, string? Cursor, bool HasMore);
