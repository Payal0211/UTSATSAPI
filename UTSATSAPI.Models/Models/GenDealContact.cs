using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenDealContact
    {
        public long Id { get; set; }
        public long? DealId { get; set; }
        public long? ContactId { get; set; }
        public long? CompanyId { get; set; }
    }
}
