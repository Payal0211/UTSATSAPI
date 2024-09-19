using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentCommunicationSkill
    {
        public int Id { get; set; }
        public string? CommunicationSkill { get; set; }
        public bool? IsActive { get; set; }
    }
}
