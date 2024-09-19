using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgHiringRequestRoleStatus
    {
        public int Id { get; set; }
        public string? HiringRequestRoleStatus { get; set; }
        public int? HrStatusId { get; set; }
        public bool? IsActive { get; set; }
        public string? Color { get; set; }
    }
}
