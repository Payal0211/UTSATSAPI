using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_HiringRequest_History_Insert
    {
        public string Action { get; set; }
        public int HiringRequest_ID { get; set; }
        public int Talent_ID { get; set; }
        public bool Created_From { get; set; }
        public int CreatedById { get; set; }
        public int ContactTalentPriority_ID { get; set; }
        public int InterviewMaster_ID { get; set; }
        public string HR_AcceptedDateTime { get; set; }
        public int OnBoard_ID { get; set; }
        public bool IsManagedByClient { get; set; }
        public bool IsManagedByTalent { get; set; }

    }
}
