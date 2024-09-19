using Microsoft.AspNetCore.Mvc;

namespace UTSATSAPI.Models.ViewModels
{
    public class ViewAllCompanyViewModel
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public FilterFields_CompanyList? filterFields_CompanyList { get; set; }
    }
    public class FilterFields_CompanyList
    {
        public string? Company { get; set; }
        public string? CompanyDomain { get; set; }
        public string? Location { get; set; }
        public string? Contact_Status { get; set; }
        public string? GEO { get; set; }
        public string? AM_SalesPerson { get; set; }
        public string? NBD_SalesPerson { get; set; }
        public string? TeamLead { get; set; }
        public string? Lead_Type { get; set; }
        public string? LeadUser { get; set; }
    }
}
