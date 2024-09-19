using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentComfortableWorkingDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? ComfortableWorkingId { get; set; }
    }
}
