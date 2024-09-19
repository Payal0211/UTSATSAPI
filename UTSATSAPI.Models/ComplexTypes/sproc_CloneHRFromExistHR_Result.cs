using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_CloneHRFromExistHR_Result
    {
        public long CloneHRID { get; set; }
        public string? CloneHRGuid { get; set; }
    }
}
