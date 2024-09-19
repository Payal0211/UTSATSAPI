using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAMAssignments_Result
    {
        public long ID { get; set; }
        public string? UserName { get; set; }
        public string? GEOName { get; set; }
        public int? Priority { get; set; }
        public int TotalRecords { get; set; }
        public bool IsDeleted { get; set; }
    }
}
