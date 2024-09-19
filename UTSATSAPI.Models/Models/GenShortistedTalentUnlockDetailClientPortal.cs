using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenShortistedTalentUnlockDetailClientPortal
    {
        public long Id { get; set; }
        public long? AtsTalentId { get; set; }
        public long? Hrid { get; set; }
        public long? ContactId { get; set; }
        public string? Guid { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
