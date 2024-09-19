using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_get_UTS_GetFilterTypeForDeals_Result
    {
        public string? FullName { get; set; }
        public long? ID { get; set; }
    }
}
