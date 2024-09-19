using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTeam
    {
        public long Id { get; set; }
        public long? DeptId { get; set; }
        public string? Team { get; set; }
        public long? TeamHeadId { get; set; }
        public long? UserTypeId { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTeamForNewUser { get; set; }
        public long? TeamRoleId { get; set; }
    }
}
