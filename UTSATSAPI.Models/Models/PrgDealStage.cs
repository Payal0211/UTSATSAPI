using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgDealStage
    {
        public int Id { get; set; }
        public string Stage { get; set; } = null!;
        public string PipelineId { get; set; } = null!;
        public string? HubSpotStage { get; set; }
        public bool? IsDisplay { get; set; }
        public string? ColorCode { get; set; }
    }
}
