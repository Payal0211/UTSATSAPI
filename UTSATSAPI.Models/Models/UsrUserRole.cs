using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class UsrUserRole
    {
        public int Id { get; set; }
        public int? UserTeamId { get; set; }
        public string? UserRole { get; set; }
        public string? LeadType { get; set; }
        public bool? IsNew { get; set; }
        public bool? IsActive { get; set; }
        public long? RoleLevelId { get; set; }
        public int? GroupRoleId { get; set; }
    }
}
