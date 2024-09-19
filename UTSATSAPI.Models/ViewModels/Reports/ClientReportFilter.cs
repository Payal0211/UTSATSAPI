namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class ClientReportFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? CompanyCategory { get; set; }
        public string? SalesManagerID { get; set; }
    }

    public class ClientBasedReport_PopUp
    {
        public string Client { get; set; }
        public string Company { get; set; }
        public string SalesUser { get; set; }
        public string Hr_Number { get; set; }
        public string Talent { get; set; }
        public string Status { get; set; }

        public string ClientStage { get; set; }
        public ClientReportFilter ReportFilter { get; set; }
    }
}
