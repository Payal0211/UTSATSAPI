using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_TalentHR_CancelledHR_List_Result
    {
        public long? HRID { get; set; }
        public int HRStatusID { get; set; }
        public string? HRStatus { get; set; }
        public long? TalentID { get; set; }
        public decimal? FinalCost { get; set; }
        public long? ATS_Talent_ID { get; set; }
        public string? TalentStatus { get; set; }
        public string? Reason { get; set; }
    }
}
