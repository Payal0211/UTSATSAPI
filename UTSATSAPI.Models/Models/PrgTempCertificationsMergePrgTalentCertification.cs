using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgTempCertificationsMergePrgTalentCertification
    {
        public long Id { get; set; }
        public long? TempCertificationId { get; set; }
        public long? PrgTalentCertificationId { get; set; }
        public int? MergeBy { get; set; }
        public DateTime? MergeDatetime { get; set; }
    }
}
