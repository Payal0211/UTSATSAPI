using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgOnBoardLegalKickOffStatus
    {
        public int Id { get; set; }
        public string? LegalKickOffStatus { get; set; }
        public bool? IsLegal { get; set; }
        public bool? IsKickOff { get; set; }
        public bool? IsActive { get; set; }
    }
}
