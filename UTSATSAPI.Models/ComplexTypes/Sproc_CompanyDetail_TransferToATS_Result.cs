using Microsoft.EntityFrameworkCore;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_CompanyDetail_TransferToATS_Result
    {
        public long ID { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public Nullable<int> domain_id { get; set; }
        public string? LinkedInProfile { get; set; }
        public Nullable<int> CompanySize { get; set; }
        public Nullable<int> TimeZone_ID { get; set; }
        public Nullable<int> CurrencyID { get; set; }
        public string? Address { get; set; }
        public string? phone { get; set; }
        public string? industry { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? country { get; set; }
        public string? zip { get; set; }
        public Nullable<bool> IsNDASigned { get; set; }
        public Nullable<int> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedByDatetime { get; set; }
        public Nullable<int> ModifiedByID { get; set; }
        public Nullable<System.DateTime> ModifiedByDatetime { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string? Location { get; set; }
        public Nullable<int> Client_StatusID { get; set; }
        public Nullable<int> TeamManagement { get; set; }
        public Nullable<decimal> Score { get; set; }
        public string? Category { get; set; }
        public Nullable<int> GEO_ID { get; set; }
        public Nullable<int> AM_SalesPersonID { get; set; }
        public Nullable<int> NBD_SalesPersonID { get; set; }
        public string? Discovery_Call { get; set; }
        public string? Lead_Type { get; set; }
        public string? CompanyLogo { get; set; }
        public string? CompanyIndustry_Type { get; set; }
        public string? CompanyGEO { get; set; }
        public string? AboutCompany { get; set; }
        public int? HRTypeID { get; set; }
        public string? HRTypeText { get; set; }
        public int? AnotherHRTypeID { get; set; } 
        public string? AnotherHRTypeText { get; set; }
        public int? VettedProfileViewCredit { get; set; }
        public int? NonVettedProfileViewCredit { get; set; }
        public bool? IsHybrid { get; set; }
        public string? FoundedYear { get; set; }
        public string? CompanyType { get; set; }
        public string? Headquaters { get; set; }
        public string? Culture { get; set; }
        public bool? IsSelfFunded { get; set; }
        public string? TotalFundings { get; set; }
        public string? CompanyNumber { get; set; }
        public int? JobPostCredit { get; set; }
        public bool? IsProfileView { get; set; }
        public bool? IsPostaJob { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CreditCurrency { get; set; }
        public decimal? JPCreditBalance { get; set; }
    }
}
