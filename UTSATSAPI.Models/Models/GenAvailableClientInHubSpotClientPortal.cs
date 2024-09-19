using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAvailableClientInHubSpotClientPortal
    {
        public long Id { get; set; }
        public long? HubSpotContactId { get; set; }
        public long? HubSpotCompanyId { get; set; }
        public long? CompanyId { get; set; }
        public long? ContactId { get; set; }
        public string? ClientEmail { get; set; }
        public string? Company { get; set; }
        public string? ClientName { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
