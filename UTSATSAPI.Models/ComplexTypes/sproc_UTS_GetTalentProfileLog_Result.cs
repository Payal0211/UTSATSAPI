using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetTalentProfileLog_Result
    {
        public long TalentID { get; set; }
        public string TalentName { get; set; }
        public string TalentRole { get; set; }
        public int ProfileSharedCount { get; set; }
        public int FeedbackCount { get; set; }
        public int RejectedCount { get; set; }
        public int SelectedForCount { get; set; }

    }
}
