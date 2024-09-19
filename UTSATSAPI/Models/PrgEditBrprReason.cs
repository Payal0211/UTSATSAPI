using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgEditBrprReason
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
