using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
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
