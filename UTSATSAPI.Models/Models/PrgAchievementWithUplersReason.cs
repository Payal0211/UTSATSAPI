using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgAchievementWithUplersReason
    {
        public long Id { get; set; }
        public string? Reason { get; set; }
        public bool? IsActive { get; set; }
        public string? Hrtype { get; set; }
    }
}
