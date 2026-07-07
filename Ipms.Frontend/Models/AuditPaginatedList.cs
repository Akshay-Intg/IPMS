using Microsoft.EntityFrameworkCore;

namespace Ipms.Frontend.Models
{
    public class AuditPaginatedList<T>
    {
        public List<T> AuditLogList { get; set; } = new();
        public int TotalAudits { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public AuditPaginatedList(List<T> auditLogList,int count,int pageIndex,int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages=(int)Math.Ceiling(count/(double)pageSize);
            AuditLogList = auditLogList;
            TotalAudits = count;
            PageSize = pageSize;
        }
        public bool HasPreviousPage=>(PageIndex>1);
        public bool HasNextPage => (PageIndex < TotalPages);
        public int FirstItemIndex => (PageIndex - 1) * PageSize + 1;
        public int LastItemIndex =>Math.Min(PageIndex*PageSize, TotalAudits);
        public static Task<AuditPaginatedList<T>> CreateAsync(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;

            var items = source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Task.FromResult(
                new AuditPaginatedList<T>(items, count, pageIndex, pageSize)
            );
        }

    }
}
