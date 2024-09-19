using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgMatchMakingSubFactor
    {
        public int Id { get; set; }
        public int? FactorId { get; set; }
        public string? SubFactor { get; set; }
        public decimal? MultiplyingFactor { get; set; }
        public string? MappingColumn { get; set; }
        public string? MappingTable { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
