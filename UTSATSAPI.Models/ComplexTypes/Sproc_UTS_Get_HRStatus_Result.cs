using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_HRStatus_Result
    {
        public long? HRID { get; set; }
        public string? HR_Number { get; set; }
        public string? HRStatus { get; set;}
        public int? HRStatusCode { get; set; }
    }
}
