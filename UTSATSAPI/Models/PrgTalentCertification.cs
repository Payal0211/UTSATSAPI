using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class PrgTalentCertification
    {
        public int Id { get; set; }
        public string? Certifications { get; set; }
        public string? ImageName { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public int? LastModifiedById { get; set; }
        public DateTime? LastModifiedDatetime { get; set; }
    }
}
