using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenOnBoardClientLeavePolicy
    {
        public long Id { get; set; }
        public long? OnBoardId { get; set; }
        public string? Occasion { get; set; }
        public string? Day { get; set; }
        public string? Date { get; set; }
        public bool? IsActive { get; set; }
    }
}
