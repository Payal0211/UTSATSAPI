using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class UsrUserTypeAndRoleDetail
    {
        public long Id { get; set; }
        public int? UserTypeId { get; set; }
        public int? UserRoleId { get; set; }
    }
}
