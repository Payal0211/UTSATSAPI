using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgRateCard
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? HrTimeZoneId { get; set; }
        public string? TalentCategory { get; set; }
        public decimal? ClientCost { get; set; }
        public decimal? TalentCost { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
        public bool? IsActive { get; set; }
    }
}
