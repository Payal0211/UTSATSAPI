using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_ViewTalent_Result
    {
        public long ID { get; set; }
        public string? Name { get; set; }
        public string? EmailID { get; set; }
        public int? TotalRecords { get; set; }
        public string? TalentRole { get; set; }
        public int? TalentStatusID { get; set; }
        public string? TalentStatus { get; set; }
        public string? TalentCost { get; set; }
        public decimal? VersantScore { get; set; }
        public decimal? TechScore { get; set; }
        public int? FrontStatusID { get; set; }
        public decimal? CurrentCTC { get; set; }
    }
}
