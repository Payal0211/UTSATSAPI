using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgAmdealContact
    {
        public long Id { get; set; }
        public long? SalesPersonId { get; set; }
        public long? HubSpotCustomerId { get; set; }
        public long? DealId { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
