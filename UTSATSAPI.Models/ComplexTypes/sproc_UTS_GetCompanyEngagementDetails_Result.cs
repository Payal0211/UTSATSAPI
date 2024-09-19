using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetCompanyEngagementDetails_Result
    {
        public long? CompanyID { get; set; }
        public int? CompanyTypeID { get; set; }
        public int? AnotherCompanyTypeID { get; set; }
        public bool? IsPostaJob { get; set; }
        public bool? IsProfileView { get; set; }
        public decimal? JPCreditBalance { get; set; }
        public decimal? TotalCreditBalance { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public bool? IsVettedProfile { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public int? JobPostCredit { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }
        public int? HiringTypePricingId { get; set; }
    }
}
