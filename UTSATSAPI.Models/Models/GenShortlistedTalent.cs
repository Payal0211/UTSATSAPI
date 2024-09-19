using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenShortlistedTalent
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? HiringRequestDetailId { get; set; }
        public long? ContactId { get; set; }
        public long? TalentId { get; set; }
        public bool? IsInterested { get; set; }
        public bool? IsPostAacceptanceConfirm { get; set; }
        public bool? IsSeenByTalent { get; set; }
        public DateTime? SeenDatetime { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public string? ClientReadytoworkShift { get; set; }
        public string? TalentReadytoworkShift { get; set; }
        public string? ClientReadytoworkhrs { get; set; }
        public string? TalentReadytoworkhrs { get; set; }
        public string? ClientJoinWithin { get; set; }
        public string? TalentJoinWithin { get; set; }
        public bool? IsConfirmed { get; set; }
    }
}
