using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgEditBrprReason
    {
        public int Id { get; set; }
        public string Reason { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
