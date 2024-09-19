using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ReplacementReportFilter
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public FilterFields? filterFields { get; set; }

    }

    public class FilterFields
    {       
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? searchText { get; set; }
        
    }

    public class TalentBackoutReportFilter
    {
        public int totalrecord { get; set; }
        public int pagenumber { get; set; }
        public FilterFields? filterFields { get; set; }

    }
}
