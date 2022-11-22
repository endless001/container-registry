namespace ContainerRegistry.Core.Models;

public class PagedList<TEntity> where TEntity : class
{
    public int PageIndex { get; init; }

    public int PageSize { get; init; }

    public long Count { get; init; }

    public IEnumerable<TEntity> Data { get; init; }

    public PagedList(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }
}