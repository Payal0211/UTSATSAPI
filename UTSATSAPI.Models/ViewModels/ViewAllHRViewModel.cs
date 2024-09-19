using Microsoft.AspNetCore.Mvc.Rendering;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Models.ViewModels
{
    public class ViewAllHRViewModel
    {
        public int Pagesize { get; set; }
        public int Pagenum { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public string? searchText { get; set; }
        public bool? IsHrfocused { get; set; }
        public bool? StarNextWeek { get; set; }
        public bool? IsDirectHR { get; set; }
        public bool? IsFrontEndHR { get; set; }
        public string? HRTypeIds { get; set; }
        public long? LeadUserID { get; set; } = 0;
        public string? HRTypeName { get; set; }
        
        

        public FilterFields_ViewAllHR? FilterFields_ViewAllHRs { get; set; }
    }

    public class FilterFields_ViewAllHR
    {
        public string? IsPoolODRBoth { get; set; }
        public string? Tenure { get; set; }
        public string? HR { get; set; }
        public string? TR { get; set; }
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string? TimeZone { get; set; }
        public string? TypeOfEmployee { get; set; }
        public string? SalesRep { get; set; }
        public string? HRStatus { get; set; }
        public string? Manager { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? HRType { get; set; }
        public string? CompanyTypeIds { get; set; }
        public string? Geos { get; set; }
    }

    public class HRFilterListViewModel
    {
        public List<SelectListItem>? Companies { get; set; }
        public List<SelectListItem>? Positions { get; set; }
        public List<SelectListItem>? Managers { get; set; }
        public List<SelectListItem>? SalesReps { get; set; }
        public List<SelectListItem>? HRTypes { get; set; }
        public List<SelectListItem>? LeadTypeList { get; set; }
        public List<SelectListItem>? CompanyModel { get; set; }
        public List<PrgHrStatusDisplay>? HrStatusList { get; set; }
        public List<SelectListItem>? GeoList { get; set; }

    }
}
