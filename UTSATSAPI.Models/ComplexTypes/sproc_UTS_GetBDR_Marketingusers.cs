using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetBDR_Marketingusers
    {
        public string Fullname { get; set; }

        public long ID { get; set; }
    }
}
