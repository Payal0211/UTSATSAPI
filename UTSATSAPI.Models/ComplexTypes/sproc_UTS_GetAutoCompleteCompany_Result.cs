using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAutoCompleteCompany_Result
    {
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
    }
}
