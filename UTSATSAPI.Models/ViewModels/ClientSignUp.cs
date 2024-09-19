using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ClientSignUp
    {
        public string? FullName { get; set; }
        public string? WorkEmail { get; set; }
        public string? CompanyName { get; set; }
        public string? Password { get; set; }
        public long? CompanyId { get; set; }
        public string? ClientIPAddress { get; set; }
        public string? OTP { get; set; }
        public int? FreeCredit { get; set; }
        public int? CompanyTypeId { get; set; }
        public long InvitingUserId { get; set; }
        public long? CookieId { get; set; }
        public string? CompanyURL { get; set; }
        public int? CompanySize { get; set; }
        public string? CompanyIndustryType { get; set; }
        public string? BriefAboutCompany { get; set; }
        public string? UTMCountry { get; set; }
        public string? UTMState { get; set; }
        public string? UTMCity { get; set; }
        public string? UTMBrowser { get; set; }
        public string? UTMDevice { get; set; }
        public string? ContactNumber { get; set; }
        public bool? IsPostaJob { get; set; } 
        public bool? IsProfileView { get; set; }
        public bool? IsHybridModel { get; set; } 
        public bool? IsVettedProfile { get; set; }
        public int? POC_ID { get; set; }
        public string? CompanyLogo { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public int? JobPostCredit { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public bool? IsPayPerCredit { get; set; }
        public bool? IsPayPerHire { get; set; }
        public string? Culture { get; set; }
        public string? FoundedYear { get; set; }
        public string? CompanyType { get; set; }
        public string? Headquaters { get; set; }
        public string? CompanySize_RangeorAdhoc { get; set; }
    }

    public class UpdateSpaceIDForClient
    {
        public string clientEmail { get; set; }
        public string SpaceID { get; set; }
        public string TokenObject { get; set; }
    }
}
