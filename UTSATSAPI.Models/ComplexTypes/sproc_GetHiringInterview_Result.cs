
namespace UTSATSAPI.Models.ComplexTypes
{
    using Microsoft.EntityFrameworkCore;

    [Keyless]
    public class sproc_GetHiringInterview_Result
    {
        public long ID { get; set; }
        public string iDate { get; set; }
        public string HRID { get; set; }
        public string ISTSlotconfirmed { get; set; }
        public string Companyname { get; set; }
        public string InterviewTimeZone { get; set; }
        public string TalentName { get; set; }
        public string InterviewStatus { get; set; }
        public string ClientStatus { get; set; }
        public int TotalRecords { get; set; }
        public int InterviewStatusFrontCode { get; set; }
        public int ClientStatusFrontCode { get; set; }
    }
}
