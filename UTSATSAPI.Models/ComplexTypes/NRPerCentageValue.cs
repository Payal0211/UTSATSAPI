using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class NRPerCentageValue
    {
        public decimal NR_Percentage { get; set; }
    }
}
