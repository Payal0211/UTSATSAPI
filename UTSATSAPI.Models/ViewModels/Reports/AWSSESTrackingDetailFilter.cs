using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class AWSSESTrackingDetailFilter
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public long? CompanyID { get; set; }
        public long? SubjectID { get; set; }
    }
    public class AWSSESTrackingDetail_Popup_Filter
    {
        public string? EventType { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public long? CompanyID { get; set; }
        public long? SubjectID { get; set; }
    }
}
