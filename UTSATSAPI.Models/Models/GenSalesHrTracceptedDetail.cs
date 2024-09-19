using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSalesHrTracceptedDetail
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public int? TrAccepted { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
