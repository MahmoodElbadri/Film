namespace Film.Domain.Shared;

public record PagedResult<T>
(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize
);
