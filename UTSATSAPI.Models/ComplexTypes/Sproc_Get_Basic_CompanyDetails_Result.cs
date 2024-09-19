using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Basic_CompanyDetails_Result
    {
        public string? CompanyName { get; set; }
        public string? Website { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? CompanyLogo {  get; set; }
        public string? FoundedYear { get; set; }
        public string? TeamSize { get; set; }
        public string? CompanyType { get; set; }
        public string? CompanyIndustry { get; set; }
        public string? Headquaters { get; set; }
        public string? AboutCompany { get; set; }
        public string? Culture { get; set; }
        public bool? IsSelfFunded { get; set; }
        public int? CompanySize { get; set; }
    }
}
