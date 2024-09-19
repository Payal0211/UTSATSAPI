using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class TrackingLeadDetailFilter
    {
        public int? NoOfJobs { get; set; }
        public string? Fromdate { get; set; }
        public string? ToDate { get; set; }
        public string? UTM_Source { get; set; }
        public string? UTM_Medium { get; set; }
        public string? UTM_Campaign { get; set; }
        public string? UTM_Content { get; set; }
        public string? UTM_Term { get; set; }
        public string? UTM_Placement { get; set; }
        public string? ref_url { get; set; }
    }
    public class TrackingLeadDetailFilterViewModel
    {
        public List<SelectListItem>? Get_JobPostCount_For_UTM_Tracking_Lead { get; set; }
        public List<SelectListItem>? UTM_Source { get; set; }
        public List<SelectListItem>? UTM_Campaign { get; set; }
        public List<SelectListItem>? UTM_Content { get; set; }
        public List<SelectListItem>? UTM_Medium { get; set; }
        public List<SelectListItem>? UTM_Placement { get; set; }
        public List<SelectListItem>? UTM_Term { get; set; }
        public List<SelectListItem>? Ref_Url { get; set; }
    }
    public class TrackingLeadReport_Details_PopUPViewModel
    {
        public int? NoOfJobs { get; set; }
        public string? Fromdate { get; set; }
        public string? ToDate { get; set; }
        public string? UTM_Source { get; set; }
        public string? UTM_Medium { get; set; }
        public string? UTM_Campaign { get; set; }
        public string? UTM_Content { get; set; }
        public string? UTM_Term { get; set; }
        public string? UTM_Placement { get; set; }
        public string? ref_url { get; set; }
        public string? Stage { get; set; }
        
    }
}
