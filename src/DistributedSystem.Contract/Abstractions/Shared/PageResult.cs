using Microsoft.EntityFrameworkCore;

namespace DistributedSystem.Contract.Abtractions.Shared;

public class PageResult<T>
{
    public const int UpperRangePageSize = 100;
    public const int DefaultPageSize = 10;
    public const int DefaultPageIndex = 1;

    public List<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasNextPage => PageIndex * PageSize < TotalCount;
    public bool HasPreviousPage => PageIndex > 1;

    private PageResult(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public static PageResult<T> Create(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        return new PageResult<T>(items, pageIndex, pageSize, totalCount);
    }

    public static async Task<PageResult<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize) 
    {         
        pageIndex = pageIndex <= 0 ? DefaultPageIndex : pageIndex;
        pageSize = pageSize <= 0 
            ? DefaultPageSize
            : pageSize > UpperRangePageSize 
            ? UpperRangePageSize : pageSize;
        
        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PageResult<T>(items, pageIndex, pageSize, totalCount);
    }
}
