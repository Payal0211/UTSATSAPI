using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSalesHrwiseContactIdforAt
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HrwiseContactId { get; set; }
        public string? Guid { get; set; }
        public bool? ShowEmailToTalent { get; set; }
        public bool? ShowContactNumberToTalent { get; set; }
    }
}
