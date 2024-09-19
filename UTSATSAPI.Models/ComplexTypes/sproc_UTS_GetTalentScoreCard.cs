namespace UTSATSAPI.Models.ComplexTypes
{
    using Microsoft.EntityFrameworkCore;
    [Keyless]
    public class sproc_UTS_GetTalentScoreCard_Result
    {
        public string SkillTest { get; set; }
        public string result { get; set; }
        public decimal Score { get; set; }
        public int Attempts { get; set; }
        public string Report { get; set; }
    }
}
