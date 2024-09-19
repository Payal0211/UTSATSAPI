namespace UTSATSAPI.Models.ViewModels
{
    public class IncentiveTargetListFilter
    {
        public int? PageIndex { get; set; } 
        public int? PageSize { get; set; } 
        public string? SortExpression { get; set; } 
        public string? SortDirection { get; set; } 
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public long? ParentId { get; set; }
        public string? targetMonthYear { get; set; }
    }
}
