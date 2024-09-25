using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Update_Basic_CompanyDetails_Result
    {
        public long? CompanyID { get; set; }
    }
}
