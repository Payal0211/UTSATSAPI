using Microsoft.EntityFrameworkCore;
namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAMAssignmentRules_Result
    {
        public long Id { get; set; }
        public int? Priority { get; set; }
        public string? Description { get; set; }
        public bool? IsAssignToBAU { get; set; }
        public bool? IsAssignToUTS { get; set; }
        public int TotalRecords { get; set; }
    }
}
