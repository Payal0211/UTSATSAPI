using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class BasicCompanyDetails
    {
        public string? Company_Size { get; set; }
        public string? Company_Industry_Type { get; set; }
        public string? Brief_About_Company { get; set; }
        public string? Headquarters { get; set; }
        public string? Company_Type { get; set; }
        public string? Founded_In { get; set; }        
        public string? CompanyLogo { get; set; }
        public string? Brief_About_Culture { get; set; }
    }
    public class OtherCompanyDetails
    {
        public List<FundingDetails> Funding_Details { get; set; }
        public List<Culture_Details> CultureDetails { get; set; }
        public List<string> Perks { get; set; }
    }
    public class AllCompanyDetails
    {
        public string? Company_Size { get; set; }
        public string? Company_Industry_Type { get; set; }
        public string? Brief_About_Company { get; set; }
        public string? Ratings { get; set; }
        public string? Reviews { get; set; }
        public string? Headquarters { get; set; }
        public string? Company_Type { get; set; }
        public string? Founded_In { get; set; }
        public List<FundingDetails> Funding_Details { get; set; }
        public List<Culture_Details> CultureDetails { get; set; }
        public string? Brief_About_Culture { get; set; }
        public List<string> Perks { get; set; }
        public string? CompanyLogo { get; set; }
    }

    public class FundingDetails
    {
        public string? Type { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }
        public string? Funding_Amount { get; set; }
        public string? Funding_Round { get; set; }
        public string? Investors { get; set; }

    }

    public class Culture_Details
    {
        public string? Culture_Images { get; set; }
    }
}
