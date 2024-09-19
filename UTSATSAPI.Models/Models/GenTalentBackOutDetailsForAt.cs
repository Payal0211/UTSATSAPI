using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentBackOutDetailsForAt
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public long? AtstalentId { get; set; }
        public string? TalentStatus { get; set; }
        public string? EngagementId { get; set; }
        public string? EngagementStartDate { get; set; }
        public string? EngagementEndDate { get; set; }
        public string? EngagementStatus { get; set; }
        public string? HiredDate { get; set; }
        public string? LostDate { get; set; }
        public string? LastWorkingDate { get; set; }
        public bool? IsSent { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
