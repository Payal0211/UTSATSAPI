using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class HRLostReportFilter
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
       
        public FilterFeild_HRLost? filterFeild_HRLost { get; set; }

    }

    public class FilterFeild_HRLost
    {
        public string? HRLostFromDate { get; set; }
        public string? HRLostToDate { get; set; }
        public string? LostReason { get; set; }
        public string? SalesUser { get; set; }
        public string? Client { get; set; }
        public string? searchText { get; set; }
    }
    public class TalentDetail
    {
        public string? TalentName { get; set; }
        public string? TalentEmail { get; set;}
        public string? TalentStatus { get; set; }
    }
}
