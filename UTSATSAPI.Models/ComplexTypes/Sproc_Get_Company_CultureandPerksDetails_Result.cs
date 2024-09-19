using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Company_CultureandPerksDetails_Result
    {
        public long? CultureID { get; set; }
        public string? CultureImage { get; set; }
    }
}
