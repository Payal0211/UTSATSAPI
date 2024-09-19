using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgDepartment
    {
        public long Id { get; set; }
        public string DeptName { get; set; } = null!;
        public bool? IsActive { get; set; }
    }
}
