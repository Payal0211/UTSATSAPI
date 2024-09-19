using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentPointofContact
    {
        public int Id { get; set; }
        public long? TalentId { get; set; }
        public int? UserId { get; set; }
        public string? Description { get; set; }
        public string? UserEmployeeId { get; set; }
    }
}
