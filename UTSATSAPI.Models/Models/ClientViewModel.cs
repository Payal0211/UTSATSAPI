using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.ViewModel;

namespace UTSATSAPI.Models.Models
{
    public class ClientViewModel
    {
        public CompanyDetails? CompanyDetails { get; set; }
        public List<ContactDetails>? ContactDetails { get; set; }
        public List<GenContactPointofContact> contactPoc { get; set; }
        public CompanyContract? CompanyContract { get; set; }

    }

    public class CompanyDetails
    {
        public long? ID { get; set; }
        public string? en_Id { get; set; }
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

        // Below properties added by Riya for UTS-6930
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public int? JobPostCredit { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }
    }

    public class ContactDetails
    {
        public long? ID { get; set; }
        public string en_Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmailID { get; set; }
        public string? Designation { get; set; }
        public string? LinkedIn { get; set; }
        public string? ContactNo { get; set; }
        public long? CompanyID { get; set; }
        public bool? IsPrimary { get; set; }
        public string? ClientProfilePic { get; set; }
        public bool? ResendInviteEmail { get; set; }
        public int? RoleID { get; set; }
    }

    public class CompanyContract
    {
        public long Id { get; set; }
        public string en_Id { get; set; }
        public long? CompanyId { get; set; }
        public string? SigningAuthorityFirstName { get; set; }
        public string? SigningAuthorityLastName { get; set; }
        public string? SigningAuthorityName { get; set; }
        public string? SigningAuthorityEmail { get; set; }
        public string? SigningAuthorityDesignation { get; set; }
        public string? LegalCompanyName { get; set; }
        public string? LegalCompanyAddress { get; set; }
    }


}
