using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgHistoryAction
    {
        public long Id { get; set; }
        public string ActionName { get; set; } = null!;
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public string? DisplayName { get; set; }
        public int? Sequence { get; set; }
        public bool IsTalentAction { get; set; }
        public bool? IsDisplayInActivity { get; set; }
        public string? Ctaname { get; set; }
    }
}
