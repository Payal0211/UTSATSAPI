using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenPayPerViewModelClientPortal
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public decimal? CreditBalance { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
    }
}
