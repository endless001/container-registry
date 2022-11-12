namespace ContainerRegistry.Core.Models;

public class PaginatedItems<TEntity> where TEntity : class
{
    public int PageIndex { get; }

    public int PageSize { get; }

    public long Count { get; }

    public IEnumerable<TEntity> Data { get; }

    public PaginatedItems(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        Count = count;
        Data = data;
    }
}