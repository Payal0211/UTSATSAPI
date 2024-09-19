using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_UTS_Get_Inc_AM_NRSlab_Result
    {
        public int ID { get; set; }
        public string? NRSlab { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
        public int TotalRecords { get; set; }
    }
}
