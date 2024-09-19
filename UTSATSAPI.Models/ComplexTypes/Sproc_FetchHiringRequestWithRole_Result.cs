using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.ComplexTypes
{
    [Keyless]
    public class Sproc_FetchHiringRequestWithRole_Result
    {
        public int ID { get; set; }
        public string TalentRole { get; set; }
        public Nullable<long> SalesHiringRequestId { get; set; }
        public Nullable<long> HiringRequest_ID { get; set; }
        public Nullable<int> Role_ID { get; set; }
        public Nullable<int> NoofEmployee { get; set; }
        public Nullable<decimal> Duration { get; set; }
        public string DurationType { get; set; }
        public string Cost { get; set; }
        public Nullable<int> RoleStatus_ID { get; set; }
        public string HiringRequest_RoleStatus { get; set; }
        public long HiringRequestDetailId { get; set; }
    }
}
