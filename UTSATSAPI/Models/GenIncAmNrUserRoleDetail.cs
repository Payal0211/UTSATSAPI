using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenIncAmNrUserRoleDetail
    {
        public int Id { get; set; }
        public int? ContractSlabId { get; set; }
        public int? UserRoleId { get; set; }
        public decimal? IncPercentage { get; set; }
        public decimal? IncAmountUsd { get; set; }
        public decimal? IncAmountInr { get; set; }
    }
}
