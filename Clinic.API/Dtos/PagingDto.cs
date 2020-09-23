namespace API.DiscSyria.API.DataTransferObject
{
    public class PagingDto
    {
        public int totalCount { get; set; }
        public int pageSize { get; set; }
        public int totalPages { get; set; }
        public int currentPage { get; set; }
        public string prevLink { get; set; }
        public string nextLink { get; set; }
        
    }
}