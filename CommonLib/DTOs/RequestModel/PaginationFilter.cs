using System;
namespace CommonLib.DTOs.RequestModel
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        private const int MAX_SIZE = 50;

        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = MAX_SIZE;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > MAX_SIZE ? MAX_SIZE : pageSize;
        }
    }
}

