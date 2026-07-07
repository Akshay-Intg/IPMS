namespace Ipms.Frontend.Models
{
    public class PolicyListPagedViewModel
    {
        public List<PolicyListViewModel> Policies { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public string? StatusFilter { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}