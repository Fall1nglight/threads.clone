using Microsoft.EntityFrameworkCore;

namespace Threads.Api.Common.Pagination;

public static class PagedQueryableExtensions
{
    // cél: egy olyan metódus megírása, amelyet meghívhatunk olyan IQueryable<T> típusokon
    //  ahol T az adott endpoint válasz DTO-ja és egyben kiterjeszti a IPaginatedEntity interfészt
    //  így a lapozáshoz szükséges logikát nem kell minden egyes endpointnál újra leírni

    public static async Task<PagedResponse<TResponseDto>> ToPagedResponse<TResponseDto>(
        this IQueryable<TResponseDto> query,
        PagedRequest request,
        CancellationToken cancellationToken
    )
        where TResponseDto : ICursorItem
    {
        // ha a kérésben szerepel 'cursor' query-paraméter
        if (!string.IsNullOrEmpty(request.Cursor))
        {
            var decodedCursor = CursorEncoder.Decode(request.Cursor);

            // ha sikerült a dekódolás
            if (decodedCursor != null)
            {
                // kiegészítjük a query-t
                query = query.Where(x =>
                    x.CreatedAtUtc < decodedCursor.LastDate
                    || (x.CreatedAtUtc == decodedCursor.LastDate && x.Id < decodedCursor.LastId)
                );
            }
        }

        // rendezzük az adathalmazt, DateTime majd Guid alapján CSÖKKENŐ SORRENDBEN
        // kiveszünk 'PageSize'+1 darab entitást
        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .ThenByDescending(x => x.Id)
            .Take(request.PageSize + 1)
            .ToListAsync(cancellationToken);

        // van-e következő oldal?
        string? encodedCursor = null;
        var hasMore = items.Count > request.PageSize;

        // ha van következő oldal
        if (hasMore)
        {
            // töröljük a +1 entitást a listából
            items.RemoveAt(items.Count - 1);

            // kivesszük az utolsó entitást, majd elmentjük a DateTime és Guid értékeit
            var lastItem = items.Last();

            // ezeket az értékeket átadjuk a segédosztály Encode() metódusának
            // elmentjük az így kapott enkódolt cursor értékét
            encodedCursor = CursorEncoder.Encode(lastItem.CreatedAtUtc, lastItem.Id);
        }

        var response = new PagedResponse<TResponseDto>(items, encodedCursor, hasMore);
        return response;
    }
}
