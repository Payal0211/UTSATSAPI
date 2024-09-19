using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenIncContractTalentUserRoleDetail
    {
        public int Id { get; set; }
        public int? UserRoleId { get; set; }
        public decimal? IncPercentage { get; set; }
        public decimal? IncAmountUsd { get; set; }
        public decimal? IncAmountInr { get; set; }
    }
}
