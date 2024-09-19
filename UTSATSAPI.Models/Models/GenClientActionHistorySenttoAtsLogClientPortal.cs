using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenClientActionHistorySenttoAtsLogClientPortal
    {
        public long Id { get; set; }
        public long? ClientHistoryId { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public bool? IsSent { get; set; }
        public string? Guid { get; set; }
    }
}
