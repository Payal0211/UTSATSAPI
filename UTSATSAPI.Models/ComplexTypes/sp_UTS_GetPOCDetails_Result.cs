using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_GetPOCDetails_Result
    {
        public long? UserID { get; set; }
        public string? EmployeeID { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
    }
}
