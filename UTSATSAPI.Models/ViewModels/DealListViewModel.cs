namespace UTSATSAPI.Models.ViewModels
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using UTSATSAPI.Models.ComplexTypes;

    [Keyless]
    public class DealListViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? searchText { get; set; }
        public FilterFields_DealList? FilterFields_DealList { get; set; } 
    }
    public class FilterFields_DealList
    {
        public string? Deal_Id { get; set; }
        public string? Lead_Type { get; set; }
        public string? Pipeline { get; set; }
        public string? Company { get; set; }
        public string? GEO { get; set; }
        public string? BDR { get; set; }
        public string? Sales_Consultant { get; set; }
        public string? DealStage { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class DealDetailViewModel
    {
        public DealDetailViewModel()
        {
            DealStage = string.Empty;
            DealType = string.Empty;
        }
        public string DealOwner { get; set; }
        public string DealStage { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string POC { get; set; }

        public List<sproc_UTS_GetDealCompanyDetails_Result> GetDealCompanydetails { get; set; }
        public List<sproc_UTS_GetDealLeadDetails_Result> GetDealLeadDetails { get; set; }
        public List<sproc_UTS_GetDealPrimary_SecondaryClient_Result> GetDealPrimaryClient { get; set; }
        public List<sproc_UTS_GetDealPrimary_SecondaryClient_Result> GetDealSecondaryClient { get; set; }
        public List<sproc_UTS_GetDealActivity_Result> GetDealActivity { get; set; }
        public List<sproc_ViewAllHRs_Result> GetAllHRs { get; set; }
    }
}
