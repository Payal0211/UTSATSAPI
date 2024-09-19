namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class DemandFunnelReportFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? IsHiringNeedTemp { get; set; }
        public string? ModeOfWork { get; set; }
        public string? TypeOfHR { get; set; }
        public string? CompanyCategory { get; set; }
        public string? Replacement { get; set; }
        public string? Head { get; set; }
        public bool IsActionWise { get; set; } = false;
        public long? LeadUserID { get; set; } = 0;
        public bool? IsHRFocused { get; set; } = false;
        public string? Geos { get; set; }
    }

    public class Demand_HrDetailPopUpFilter
    {
        public string AdhocType { get; set; }
        public string TeamManagerName { get; set; }
        public string CurrentStage { get; set; } // HR Accepted, HR Lost etc
        public Demand_PopUpFilter HrFilter { get; set; }
        public DemandFunnelReportFilter FunnelFilter { get; set; }
        public bool IsExport { get; set; } = false;
    }

    public class Demand_PopUpFilter
    {
        public string HR_No { get; set; }
        public string SalesPerson { get; set; }
        public string CompnayName { get; set; }
        public string Role { get; set; }
        public string Managed_Self { get; set; }
        public string TalentName { get; set; }
        public string Availability { get; set; }
        public string Geos { get; set; }
    }
}
