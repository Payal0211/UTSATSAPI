using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_UpdateContactDetails_Result
    {
        public long? ContactID { get; set; }
    }
}
