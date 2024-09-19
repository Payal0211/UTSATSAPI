using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentCertificationsDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? CertificationsId { get; set; }
        public string? TempCertificationId { get; set; }
    }
}
