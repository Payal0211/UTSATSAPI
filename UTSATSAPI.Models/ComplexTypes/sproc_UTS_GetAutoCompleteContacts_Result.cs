using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_UTS_GetAutoCompleteContacts_Result
    {
        public long Row_ID { get; set; }
        public long CompanyId { get; set; }
        public long ContactId { get; set; }
        public string? Value { get; set; }
        public string? ContactName { get; set; }
        public string? Company { get; set; }
        public string? CompanyURL { get; set; }
        public string? discoveryCall { get; set; }
        public string? EmailId { get; set; }
        public string? SalesUser { get; set; }
        public string? SalesUserID { get; set; }
        public bool? IsHybrid { get; set; } = false;
        public int? CompanyTypeID { get; set; }
        public string? CompanyType { get; set; }
        public bool? IsTransparentPricing { get; set; }
        public bool? IsPostaJob { get; set; }
        public bool? IsProfileView { get; set; }
        public bool? IsVettedProfile {  get; set; }
        public string? ContactNumber { get; set; }
    }
}
