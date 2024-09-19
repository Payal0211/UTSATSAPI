using Microsoft.AspNetCore.Http;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.ViewModels.CompanyProfile
{
    public class CompanyBasicDetails
    {
        public long? CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? FoundedYear { get; set; }
        public int? CompanySize { get; set; }
        public string? CompanyType { get; set; }
        public string? Industry { get; set; }
        public string? Headquaters { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public string? Culture { get; set; }
        public bool? IsSelfFunded { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? TeamSize { get; set; }
        public bool? IsDeleteCompanyLogo { get; set; }
    }
}
