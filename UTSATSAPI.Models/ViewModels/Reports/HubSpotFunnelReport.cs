using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class HubSpotFunnelReportFilter
    {
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }        
        public string? Head { get; set; }
    }

    public class HubSpot_DealDetailPopUpFilter
    {       
        public string TeamManagerName { get; set; }
        public string Stage { get; set; } // HR Accepted, HR Lost etc
        public HubSpot_PopUpFilter HrFilter { get; set; }
        public HubSpotFunnelReportFilter FunnelFilter { get; set; }
        public bool IsExport { get; set; } = false;
    }
    public class HubSpot_PopUpFilter
    {
        public long DealID { get; set; }
        public string SalesPerson { get; set; }        
        public string StageName { get; set; }
        public string DealNumber { get; set; }
    }
}
