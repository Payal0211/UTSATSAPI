using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Company_PerksDetails_Result
    {
        public string? Perks {  get; set; }
    }
}
