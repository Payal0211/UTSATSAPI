using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgContactAction
    {
        public int Id { get; set; }
        public string? ActionName { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public string? DisplayName { get; set; }
        public int? OrderSequence { get; set; }
    }
}
