using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ListSLAFilterModel
    {

        public int totalrecord { get; set; }
        public int pagenumber { get; set; }

        public FilterFieldsSLA filterFieldsSLA { get; set; }
        public bool IsExport { get; set; }
    }

    public class FilterFieldsSLA
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public long? HRID { get; set; }
        public long Sales_ManagerID { get; set; }
        public long Ops_Lead { get; set; }
        public long SalesPerson { get; set; }
        public string Stage { get; set; }
        public int IsAdHoc { get; set; }
        public string Role { get; set; }
        public int SLAType { get; set; }
        public Int32 Type { get; set; }
        public string HR_Number { get; set; }
        public string Company { get; set; }
        public int ActionFilter { get; set; }
        public int? AMBDR { get; set; }
        public string CompanyIds { get; set; }
        public string ActionFilterIDs { get; set; }
        public string StageIDs { get; set; }
        public bool? IsHRFocused { get; set; } = false;
        public string? Sales_ManagerIDs { get; set; }
        public string? Heads { get; set; }
    }
}
