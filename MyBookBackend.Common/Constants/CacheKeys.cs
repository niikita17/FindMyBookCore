namespace MyBookBackend.Common.Constants;

public static class CacheKeys
{
    public const string Categories = "categories";
    public const string Dashboard = "dashboard";

    public static string BookById(int id)
        => $"book_{id}";

    public static string Books(
        string? search,
        int? categoryId,
        int page,
        int pageSize,
        string? sortBy)
    {
        return $"books_search_{search ?? "all"}_" +
               $"category_{categoryId?.ToString() ?? "all"}_" +
               $"page_{page}_" +
               $"size_{pageSize}_" +
               $"sort_{sortBy ?? "default"}";
    }
}