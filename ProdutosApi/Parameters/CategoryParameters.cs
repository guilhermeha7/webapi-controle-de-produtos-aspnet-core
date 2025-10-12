using ProdutosApi.Parameters.Enums;

namespace ProdutosApi.Parameters
{
    public class CategoryParameters : PaginationParameters
    {
        public string? Name { get; set; } = null;
        public CategorySort? SortBy { get; set; }
        public SortDirection SortDirection { get; set; } = SortDirection.Desc;
    }
}
