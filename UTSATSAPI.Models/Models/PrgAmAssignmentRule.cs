using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgAmAssignmentRule
    {
        public long Id { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; } = null!;
        public bool IsAssignToBau { get; set; }
        public bool IsAssignToUts { get; set; }
        public string BauUtcTagging { get; set; } = null!;
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string? EngagementModelId { get; set; }
        public string? DedicatedType { get; set; }
    }
}
