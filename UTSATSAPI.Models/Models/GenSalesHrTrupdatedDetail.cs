using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSalesHrTrupdatedDetail
    {
        public long Id { get; set; }
        public long HiringRequestId { get; set; }
        public int OriginalTr { get; set; }
        public int? UpdatedTr { get; set; }
        public bool? IsIncreased { get; set; }
        public bool? IsDecreased { get; set; }
        public string? Reason { get; set; }
        public string? AdditionalRemark { get; set; }
        public bool? IsTrlost { get; set; }
        public bool? IsTrcancel { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public int? PreviousHrstatusId { get; set; }
        public long? HistoryId { get; set; }
    }
}
