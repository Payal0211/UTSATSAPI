using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_SLA_OverAll_Summary_Report_Static_Stages_Result
    {
        public int? ID { get; set; }
        public string? SummaryStage { get; set; }
        public string? CurrentStage_ActionID { get; set; }
        public string? NextStage_ActionID { get; set; }
        public string? StageVersion { get; set; }
        public int? OrderSequence { get; set; }
        public int? SLADays { get; set; }
        public decimal? AvgSLAOverall { get; set; }
        public string? DiffOfSLAOverall { get; set; }
        public decimal? AvgSLAMissed { get; set; }
        public string? DiffOfSLAMissed { get; set; }
        public string ExtraActionsIncluded { get; set; }
    }
}
