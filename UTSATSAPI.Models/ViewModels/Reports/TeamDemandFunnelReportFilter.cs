namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class TeamDemandFunnelReportFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? SalesManagerID { get; set; }
        public string? IsHiringNeedTemp { get; set; }
        public string? ModeOfWork { get; set; }
        public string? TypeOfHR { get; set; }
        public string? CompanyCategory { get; set; }
        public bool IsActionWise { get; set; } = false;
        public long? LeadUserID { get; set; } = 0;
        public bool? IsHRFocused { get; set; } = false;
    }

    public class TeamDemand_HrDetailPopUpFilter
    {
        public string AdhocType { get; set; }
        public string SelectedRow_SalesUserName { get; set; }
        public string CurrentStage { get; set; } // HR Accepted, HR Lost etc
        public TeamDemand_PopupFilter HrFilter { get; set;}
        public TeamDemandFunnelReportFilter FunnelFilter { get; set; }
    }

    public class TeamDemand_PopupFilter
    {
        public string Availability { get; set; }
        public string CompnayName { get; set; }
        public string HR_No { get; set; }
        public string Managed_Self { get; set; }
        public string Role { get; set; }
        public string SalesPerson { get; set; }
        public string TalentName { get; set; }
    }

}
