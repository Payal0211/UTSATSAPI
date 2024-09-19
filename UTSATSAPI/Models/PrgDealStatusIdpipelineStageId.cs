using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgDealStatusIdpipelineStageId
    {
        public long Id { get; set; }
        public int? DealStageId { get; set; }
        public string? PipelineId { get; set; }
        public string? StageId { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}
