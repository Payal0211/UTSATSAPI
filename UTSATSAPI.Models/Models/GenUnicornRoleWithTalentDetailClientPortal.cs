using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenUnicornRoleWithTalentDetailClientPortal
    {
        public long Id { get; set; }
        public int? UnicornRoleId { get; set; }
        public long? TalentId { get; set; }
        public long? AtsTalentId { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
