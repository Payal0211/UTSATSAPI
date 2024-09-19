using UTSATSAPI.Models.ComplexTypes;

namespace UTSATSAPI.Models.ViewModels
{
    public class AddUpdateCompanyViewModel
    {
        public AddUpdateCompanyViewModel()
        {
            CompanyData = new Sproc_CompanyDetail_TransferToATS_Result();
            FundingDetails = new List<Sproc_Get_Company_Funding_Details_Result>();
            CultureDetails = new List<Sproc_Get_Company_CultureandPerksDetails_Result>();
            PerkDetails = new List<string>();
            ContactDetails = new List<sproc_UTS_GetContactDetails_Result>();
            PocUserName = string.Empty;
        }
        public Sproc_CompanyDetail_TransferToATS_Result CompanyData { get; set; }
        public List<Sproc_Get_Company_Funding_Details_Result> FundingDetails { get; set; }
        public List<Sproc_Get_Company_CultureandPerksDetails_Result> CultureDetails { get; set; }
        public List<string> PerkDetails { get; set; }
        public List<Sproc_Get_Company_YouTubeDetails_Result> YouTubeDetails { get; set; }
        public List<sproc_UTS_GetContactDetails_Result> ContactDetails { get; set; }
        public string? PocUserName { get; set; }
        //public CompanyData CompanyData { get; set; }
    }

    //public class CompanyData
    //{
    //    public long ID { get; set; }
    //    public string? Company { get; set; }
    //    public string? Website { get; set; }
    //    public string? Address { get; set; }
    //    public string? phone { get; set; }
    //    public string? industry { get; set; }
    //    public string? city { get; set; }
    //    public string? state { get; set; }
    //    public string? country { get; set; }
    //    public string? zip { get; set; }
    //    public Nullable<int> CurrencyID { get; set; }
    //    public Nullable<bool> IsNDASigned { get; set; }
    //    public Nullable<int> CreatedByID { get; set; }
    //    public Nullable<System.DateTime> CreatedByDatetime { get; set; }
    //    public Nullable<int> ModifiedByID { get; set; }
    //    public Nullable<System.DateTime> ModifiedByDatetime { get; set; }
    //    public Nullable<bool> IsActive { get; set; }
    //    public Nullable<int> domain_id { get; set; }
    //    public string? LinkedInProfile { get; set; }
    //    public Nullable<int> CompanySize { get; set; }
    //    public Nullable<int> TimeZone_ID { get; set; }
    //    public string? Location { get; set; }
    //    public Nullable<int> Client_StatusID { get; set; }
    //    public Nullable<int> TeamManagement { get; set; }
    //    public Nullable<decimal> Score { get; set; }
    //    public string? Category { get; set; }
    //    public Nullable<int> GEO_ID { get; set; }
    //    public Nullable<int> AM_SalesPersonID { get; set; }
    //    public Nullable<int> NBD_SalesPersonID { get; set; }
    //    public string? Discovery_Call { get; set; }
    //    public string? Lead_Type { get; set; }
    //    public string? CompanyLogo { get; set; }
    //    public string? CompanyGEO { get; set; }
    //    public string? CompanyIndustry_Type { get; set; }
    //    public string? AboutCompany { get; set; }
    //    public int? HRTypeID { get; set; }
    //    public string? HRTypeText { get; set; }
    //    public int? VettedProfileViewCredit { get; set; }
    //    public int? NonVettedProfileViewCredit { get; set; }
    //}
}