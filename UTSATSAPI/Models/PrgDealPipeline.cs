using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgDealPipeline
    {
        public int Id { get; set; }
        public string Pipeline { get; set; } = null!;
        public string? HubSpotPipeline { get; set; }
    }
}
