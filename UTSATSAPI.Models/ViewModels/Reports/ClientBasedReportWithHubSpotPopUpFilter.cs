namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class ClientBasedReportWithHubSpotPopUpFilter
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string StageName { get; set; }
        public string FullName { get; set; }
        public string Company { get; set; }
        public string GEO { get; set; }
        public string SalesUser { get; set; }
        public string Hr_Number { get; set; }
        public string Name { get; set; }
        public string CompanyCategory { get; set; }
        public long SalesManagerID { get; set; }
        public string Status { get; set; }
        public string SalesManagerIDs { get; set; }
        public long LeadUserID { get; set; }
        public bool IsHRFocused { get; set; } = false;
    }
}