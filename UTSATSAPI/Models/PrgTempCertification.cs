using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTempCertification
    {
        public long Id { get; set; }
        public string? TempCertificationId { get; set; }
        public string? TempCertification { get; set; }
        public string? ImageName { get; set; }
        public int? CertificationSourceId { get; set; }
        public string? Source { get; set; }
        public int? StatusId { get; set; }
        public bool? AddedByTalent { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? Guid { get; set; }
        public long? PrgCertificationId { get; set; }
        public string? RejectReason { get; set; }
        public bool? IsSentInEmail { get; set; }
    }
}
