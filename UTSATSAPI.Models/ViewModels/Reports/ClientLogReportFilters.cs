using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class ClientLogReportFilter: CommonFilterModel
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? LoginStatus { get; set; }
    }
    //public class ClientLogBasedReport_PopUp
    //{
    //    public string? DisplayName { get; set; }
    //    public Nullable<System.DateTime> CreatedByDatetime { get; set; }
    //    public string? Name { get; set; }
    //    public string? TName { get; set; }
    //}
}
