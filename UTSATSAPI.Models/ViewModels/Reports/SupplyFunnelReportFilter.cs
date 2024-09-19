namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class SupplyFunnelReportFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Managed { get; set; }
        public string? IsHiringNeedTemp { get; set; }
        public string? ModeOfWork { get; set; }
        public string? TypeOfHR { get; set; }
        public string? CompanyCategory { get; set; }
        public string? Replacement { get; set; }

        public bool IsActionWise { get; set; } = false;
    }

    public class Supply_HrDetailPopUpFilter
    {
        public string? NewExistingType { get; set; }
        public string? TeamManagerName { get; set; }
        public string CurrentStage { get; set; } // HR Accepted, HR Lost etc
        public SupplyFunnelReportFilter FunnelFilter { get; set; }
        public bool IsExport { get; set; } = false;
    }
}
