namespace KatieComedy.App.Pagination;

public record PageArgs
{
    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }
}

public record PagedResult<T>
{
    public required PageParameters Parameters { get; init; }

    public required IReadOnlyList<T> Items { get; init; } = [];
}

public record PageParameters
{
    public required int PageIndex { get; init; }

    public required int PageSize { get; init; }

    public required int TotalCount { get; init; }

    public int IndexMargin { get; init; } = 2;

    public bool HasPrevious => PageIndex > 1;

    public bool HasNext => PageIndex < PageCount;

    public int PageCount
    {
        get
        {
            if (PageSize == 0)
            {
                return 0;
            }

            return Convert.ToInt32(Math.Ceiling((double)TotalCount / PageSize));
        }
    }

    public int ItemOffset
    {
        get
        {
            var currentPage = Math.Min(PageIndex, PageCount);
            return (currentPage - 1) * PageSize;
        }
    }

    public IReadOnlyList<int> PageNavIndices
    {
        get
        {
            var prevIndices = Enumerable
                .Range(PageIndex - IndexMargin, IndexMargin)
                .Where(x => x > 0);

            var nextIndices = Enumerable
                .Range(PageIndex + 1, IndexMargin)
                .Where(x => x <= PageCount);

            return [.. prevIndices, PageIndex, .. nextIndices];
        }
    }
}
