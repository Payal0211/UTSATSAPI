using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgPreOnBoardStatus
    {
        public int Id { get; set; }
        public string? PreOnBoardStatus { get; set; }
        public bool? IsDisplayToClient { get; set; }
        public bool? IsDisplayToTalent { get; set; }
        public bool? IsActive { get; set; }
    }
}
