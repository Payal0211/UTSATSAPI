namespace UTSATSAPI.Models.ViewModels
{
    public class CommonFilterModel
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string? SortExpression { get; set; }
        public string? SortDirection { get; set; }
        public string? SearchText { get; set; }
    }
}
