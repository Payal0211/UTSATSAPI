using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_getCompanyNameByHiringRequestID
    {
        public string Company { get; set; }
    }
}
