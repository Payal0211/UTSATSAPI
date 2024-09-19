using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenUserHistory
    {
        public long Id { get; set; }
        public long ActionId { get; set; }
        public long UserId { get; set; }
        public long? UserTypeId { get; set; }
        public long? UserRoleId { get; set; }
        public long? ParentId { get; set; }
        public long? ReporteeUserId { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
