using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_PayOut_Basic_Information_Result
    {
        public long? PayOutID { get; set; }
        public long? OnBoardID { get; set;}
        public long? HiringRequestID { get; set; }
        public long? ContactID { get; set; }
        public long? TalentID { get; set; }
        public string? CompanyName { get; set; }
        public string? ClientName { get; set; }
        public string? TalentName { get; set; }
        public string? EngagementId_HRID { get; set; }
        public int? AM_SalesPersonID { get; set; }
        public string? AM_UserName { get; set; }
    }
}
