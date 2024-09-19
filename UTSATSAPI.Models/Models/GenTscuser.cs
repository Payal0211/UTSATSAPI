using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTscuser
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int? Priority { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
