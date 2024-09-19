namespace UTSATSAPI.Models.ViewModels
{
    public class ListClientFilter
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }

        public FilterFields_Client? filterFields_Client { get; set; }
    }
    public class FilterFields_Client
    {
        public string? CompanyStatus { get; set; }
        public string? GEO { get; set; }
        public string? AddingSource { get; set; }
        public string? Category { get; set; }
        public string? POC { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? searchText { get; set; }
        public long? LeadUserID { get; set; } = 0;
        public string? SearchSourceCategory { get; set; }
        public string? SearchCompanyModel { get; set; }
    }

    public class ViewClientDetails
    {
        public long CompanyID { get; set; }
        public long ClientID { get; set; }
    }
}
