using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTempInterest
    {
        public long Id { get; set; }
        public string? TempInterestId { get; set; }
        public string? TempInterest { get; set; }
        public string? ImageName { get; set; }
        public int? InterestSourceId { get; set; }
        public string? Source { get; set; }
        public int? StatusId { get; set; }
        public bool? AddedByTalent { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? Guid { get; set; }
        public long? PrgInterestId { get; set; }
        public string? RejectReason { get; set; }
        public bool? IsSentInEmail { get; set; }
    }
}
