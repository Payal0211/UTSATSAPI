using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentNotesClientPortal
    {
        public long Id { get; set; }
        public long? ClientId { get; set; }
        public long? Hrid { get; set; }
        public long? AtstalentId { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public string? Note { get; set; }
        public string? AtsnoteId { get; set; }
        public bool? IsManagedByClient { get; set; }
        public DateTime? LastModifiedByDateTime { get; set; }
        public int? AppActionDoneBy { get; set; }
        public bool? IsEmailSentToClient { get; set; }
        public long? CreatedById { get; set; }
    }
}
