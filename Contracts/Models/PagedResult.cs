namespace Contracts.Models;

public sealed class PagedResult<T> where T : class
{
    public List<T> Items { get; }
    public int TotalItemsCount { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }

    public PagedResult(List<T> items, int pageSize, int pageNumber)
    {
        Items = items;
        HasPreviousPage = pageNumber > 0;
        HasNextPage = items.Count >= pageSize;
    }
}
