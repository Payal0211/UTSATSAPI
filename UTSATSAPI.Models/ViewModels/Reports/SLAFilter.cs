namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class SLAFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Stage { get; set; }
        public int ActionFilter { get; set; }
        public int IsAdHoc { get; set; }
        public int AMNBD { get; set; }
        public long SalesManagerID { get; set; }
        public long SV { get; set; }
        public long SalesPerson { get; set; }
        public string? Role { get; set; }
        public string? HR_Number { get; set; }
        public string? Company { get; set; }
    }

    public class SLAReport_Filter
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? SLAType { get; set;}
        public SLAFilter sLAFilter { get; set; }
    }

}
