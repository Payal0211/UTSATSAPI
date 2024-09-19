using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgUserRoleClientPortal
    {
        public long Id { get; set; }
        public string? Role { get; set; }
        public string? RoleDescription { get; set; }
        public bool? IsActive { get; set; }
    }
}
