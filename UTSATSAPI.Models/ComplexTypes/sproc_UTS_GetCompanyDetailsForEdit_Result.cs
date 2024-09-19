using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetCompanyDetailsForEdit_Result
    {
        public long? ID { get; set; }
        public string? CompanyName { get; set; }
        public string? Website { get; set; }
        public string? LinkedInProfile { get; set; }
        public int? CompanySize { get; set; }
        public string? Location { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Zip { get; set; }
        public string? Address { get; set; }
        public int? GEO_ID { get; set; }
        public int? TeamManagement { get; set; }
        public string? CompanyLogo { get; set; }
        public string? LeadType { get; set; }
        public int? LeadUserID { get; set; }
        public string? AboutCompanyDesc { get; set; }
        public decimal? JPCreditBalance { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public int? AnotherCompanyTypeID { get; set; }
        public bool? IsPostaJob { get; set; }
        public bool? IsProfileView { get; set; }
        public int? CompanyTypeID { get; set; }
        public bool? IsVettedProfile { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public int? JobPostCredit { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }

    }
}
