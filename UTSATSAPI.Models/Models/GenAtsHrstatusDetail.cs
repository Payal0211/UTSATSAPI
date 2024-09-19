using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAtsHrstatusDetail
    {
        public long Id { get; set; }
        public long? Hrid { get; set; }
        public long? AtshrstatusId { get; set; }
        public DateTime? PublishedDatetime { get; set; }
        public long? TotalTalent { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
    }
}
