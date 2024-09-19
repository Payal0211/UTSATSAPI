using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTalentAction
    {
        public long Id { get; set; }
        public string ActionName { get; set; } = null!;
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedByDatetime { get; set; }
        public string? DisplayName { get; set; }
    }
}
