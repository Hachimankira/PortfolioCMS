namespace PortfolioCMS.DTOs
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 50 ? 50 : (value < 1 ? 10 : value);
        }
        
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public string? SortDirection { get; set; }

        public PaginationFilter()
        {
            PageNumber = 1;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize;
            SortDirection = "asc";
        }
    }
}
