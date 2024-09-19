using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenShortlistedTalentsClientPortal
    {
        public long Id { get; set; }
        public long? Hrid { get; set; }
        public long? ContactId { get; set; }
        public long? Ctpid { get; set; }
        public bool? IsBookMarked { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
    }
}
