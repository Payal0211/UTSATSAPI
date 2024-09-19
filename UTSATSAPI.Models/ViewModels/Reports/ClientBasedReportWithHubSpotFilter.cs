using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels.Reports
{
    public class ClientBasedReportWithHubSpotFilter
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public long GeoId { get; set; }
        public string CompanyCategory { get; set; }
        public long SalesMangerId { get; set; }
        public string SalesManagerIds { get; set; }
        public long LeadUserId { get; set; }
        public bool IsHRFocused { get; set; } = false;
    }
}
