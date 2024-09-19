using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_Acheived_Target_Details_Result
    {
        public long? ID { get; set; }
        public string? HRNumber { get; set; }
        public string? InvoiceNumber { get; set; }
        public string? TargetDate { get; set; }
        public string? CreatedByDatetime { get; set; }
        public int TotalRecords { get; set; }
        public string? User_Role { get; set; }
        public string? Client { get; set; }
        public string? TalentName { get; set; }
        public string? EngagemenID { get; set; }
        public string? CompanyCategory { get; set; }
        public long? HRID { get; set; }
    }
}
