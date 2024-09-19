using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgHiringRequestStatus
    {
        public int Id { get; set; }
        public string? HiringRequestStatus { get; set; }
        public bool? IsActive { get; set; }
        public string? Color { get; set; }
    }
}
