using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentRoleCategory
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? TalentCategoryId { get; set; }
    }
}
