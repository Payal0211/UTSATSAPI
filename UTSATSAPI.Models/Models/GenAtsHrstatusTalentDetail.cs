using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAtsHrstatusTalentDetail
    {
        public long Id { get; set; }
        public long? StatusId { get; set; }
        public long? HrstatusDetailsId { get; set; }
        public long? AtsTalentId { get; set; }
        public DateTime? AssociatedDateTime { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
    }
}
