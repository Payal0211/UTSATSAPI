using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgIncAmNrSlab
    {
        public int Id { get; set; }
        public string? ContractsSlab { get; set; }
        public int? MinLimitPercentage { get; set; }
        public int? MaxLimitPercentage { get; set; }
        public bool? IsActive { get; set; }
    }
}
