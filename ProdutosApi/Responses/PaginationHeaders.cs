namespace ProdutosApi.Responses
{
    public class PaginationHeaders
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public bool HasNext => CurrentPage < TotalPages; //O valor da propriedade HasNext vai ser true toda vez que CurrentPage < TotalPages
        public bool HasPrevious => CurrentPage > 1; //O valor da propriedade HasPrevious vai ser true todas vez que CurrentPage > 1
        
        public PaginationHeaders(int currentPage, int totalPages, int pageSize, int totalItems)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PageSize = pageSize;
            TotalItems = totalItems;
        }
    }
}
