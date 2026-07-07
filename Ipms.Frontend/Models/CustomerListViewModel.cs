using Ipms.Frontend.DTOs.Deserialize;

namespace Ipms.Frontend.Models
{
    public class CustomerListViewModel
    {
        public List<CustomerListDeserialize> Customers { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; } = 10;
        public string? Search { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool IsDeleted { get; set; }
    }
}