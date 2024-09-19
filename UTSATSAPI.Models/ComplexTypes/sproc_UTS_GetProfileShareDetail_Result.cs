using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetProfileShareDetail_Result
    {
        public string? HRID { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string? sDate { get; set; }
        public string? Feedbackurl { get; set; }
        public int? FrontStatusID { get; set; }
        public string? TalentStatus { get; set; }
    }
}
