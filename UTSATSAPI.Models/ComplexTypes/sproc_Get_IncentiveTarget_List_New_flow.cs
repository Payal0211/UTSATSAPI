using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_Get_IncentiveTarget_List_New_flow
    {
        public long? UserId { get; set; }
        public long? ID { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public string? MonthName { get; set; }
        public int? Year { get; set; }
        public decimal? Usertarget { get; set; }
        public decimal? SelfTarget { get; set; }
        public int? TotalRecords { get; set; }
        public string? CreatedByDatetime { get; set; }
        public int? ChildCount { get; set; }
    }
}
