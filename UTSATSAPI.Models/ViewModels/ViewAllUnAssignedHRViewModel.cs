using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ViewAllUnAssignedHRViewModel
    {
        public int Pagesize { get; set; }
        public int Pagenum { get; set; }
        public string? Sortdatafield { get; set; }
        public string? Sortorder { get; set; }
        public string? searchText { get; set; }

        public FilterFields_ViewAllUnAssignedHR? FilterFields_ViewAllUnAssignedHRs { get; set; }
    }
    public class FilterFields_ViewAllUnAssignedHR
    {       
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
    }
}
