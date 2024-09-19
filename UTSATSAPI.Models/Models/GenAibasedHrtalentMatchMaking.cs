using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAibasedHrtalentMatchMaking
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public bool? IsFavourite { get; set; }
        public bool? IsRejected { get; set; }
        public int? ActionId { get; set; }
        public decimal? MatchingPercentage { get; set; }
        public long? HiringRequestIds { get; set; }
        public bool? IsMultipleHr { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
