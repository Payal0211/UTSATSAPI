using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]

    public class sproc_Insert_HiringRequest_PriorityHistory
    {
        public long? loggedInUserId { get; set; }
        public long? salesUserID { get; set; }
        public long? hiringRequestID { get; set; }
        public bool? isPriority { get; set; }
        public Nullable<DateTime> priorityStartDate { get; set; }
        public Nullable<DateTime> priorityEndDate { get; set; }
        public bool? isNextWeekPriority { get; set; }
    }
}
