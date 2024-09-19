using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_SLA_Report_For_Static_Stages_Result
    {
        public int? ID { get; set; }
        public string? HR_NUmber { get; set; }
        public string? Company { get; set; }
        public string? Client { get; set; }
        public string? CurrentStage { get; set; }
        public string? NextStage { get; set; }
        public string? TalentName { get; set; }
        public string? Current_Action_date { get; set; }
        public string? Expected_Next_action_date { get; set; }
        public string? Actual_Next_Action_date { get; set; }
        public int? Expected_SLA_day { get; set; }
        public int? Actual_SLA_day { get; set; }
        public string? Sales_Person { get; set; }
        public string? Sales_Manager { get; set; }
        public string? Ops_Lead { get; set; }
        public string? Role { get; set; }
        public long? HRID { get; set; }
        public string? CurrentActionID { get; set; }
        public string? NextActionID { get; set; }
        public long? TalentId { get; set; }
        public string? IsAdHocHR { get; set; }
        public string? ActionFilter { get; set; }
        public int? SLA_diff { get; set; }
        public bool? IsWeekEndSkip { get; set; }
    }
}
