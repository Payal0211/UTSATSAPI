using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_GetCompanyClientDetails_Result
    {
        public string? CompanyName { get; set; }
        public string? CompanyInitial { get; set; }
        public string? ClientName { get; set; }
        public string? ClientSource { get; set; }
        public string? ClientStatus { get; set; }
        public string? UplersPOC { get; set; }
        public int? AM_SalesPersonID { get; set; }
        public string? AM_UserName { get; set; }
        public string? ClientEmail { get; set; }
        public string? ClientLinkedIn { get; set; }
        public string? CompanyURL { get; set; }
        public string? CompanyLinkedIn { get; set; }
        public string? LeadSource { get; set; }
        public string? LeadUser { get; set; }
        public string? GEO { get; set; }
        public double? TR { get; set; }
        public string? Industry { get; set; }
        public int? CompanySize { get; set; }
        public bool? AllowOffshore { get; set; }
        public string? AboutCompany { get; set; }
        public string? ViewCompanyURL { get; set; }
        public string? CompanyLogo { get; set; }
        public string? ClientContactNumber { get; set; }
    }
}
