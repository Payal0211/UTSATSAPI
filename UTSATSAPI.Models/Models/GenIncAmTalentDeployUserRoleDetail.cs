using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenIncAmTalentDeployUserRoleDetail
    {
        public int Id { get; set; }
        public int? SlabId { get; set; }
        public int? UserRoleId { get; set; }
        public decimal? IncPercentage { get; set; }
        public decimal? IncAmountUsd { get; set; }
        public decimal? IncAmountInr { get; set; }
    }
}
