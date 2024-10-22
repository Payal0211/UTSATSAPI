using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_AddEdit_ATSHR_Result
    {
        public long? HiringRequestID { get; set; }
        public string? HR_Number  { get; set; }
        public string? ResponseMsg { get; set; }
    }
}
