namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class SkillWiseReportFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool IsActionWise { get; set; }
    }

    public class SkillWise_PopupFilter
    {
        public string? SkillName { get; set;}
        public string? TypeOfCount { get; set; }
        public string? AdhocType { get; set; }

        public SkillWiseReportFilter reportFilter { get; set; }
    }
}
