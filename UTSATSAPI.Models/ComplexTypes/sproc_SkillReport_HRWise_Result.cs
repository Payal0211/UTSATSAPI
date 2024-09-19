using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_SkillReport_HRWise_Result
    {
        public string Skill { get; set; }
        public int TotalHRSubmitted { get; set; }
        public int TotalAccepted { get; set; }
        public int TotalAcceptedPool { get; set; }
        public int TotalAcceptedOdr { get; set; }
    }
}
