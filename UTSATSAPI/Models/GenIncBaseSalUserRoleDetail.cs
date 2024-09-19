using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenIncBaseSalUserRoleDetail
    {
        public int Id { get; set; }
        public int? SalgoalSlabId { get; set; }
        public int? UserRoleId { get; set; }
        public int? IncPercentage { get; set; }
        public decimal? IncAmountUsd { get; set; }
        public decimal? IncAmountInr { get; set; }
    }
}
