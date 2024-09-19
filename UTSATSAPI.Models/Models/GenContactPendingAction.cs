using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenContactPendingAction
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public string? PendingAction { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
