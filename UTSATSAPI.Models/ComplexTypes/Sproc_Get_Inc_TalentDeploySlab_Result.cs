using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Inc_TalentDeploySlab_Result
    {
        public int? ID { get; set; }
        public string? TalentDeploySlab { get; set; }
        public decimal? AM { get; set; }
        public decimal? AMHead { get; set; }
        public int? TotalRecords { get; set; }
    }
}
