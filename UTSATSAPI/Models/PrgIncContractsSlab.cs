using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgIncContractsSlab
    {
        public int Id { get; set; }
        public string? ContractMonth { get; set; }
        public int? MinLimitMonths { get; set; }
        public int? MaxLimitMonths { get; set; }
        public bool? IsActive { get; set; }
    }
}
