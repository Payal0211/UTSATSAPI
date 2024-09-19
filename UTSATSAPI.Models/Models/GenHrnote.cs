using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenHrnote
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public string? Notes { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
