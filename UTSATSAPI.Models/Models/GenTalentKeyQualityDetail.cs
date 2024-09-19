using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentKeyQualityDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public int? KeyQualitiesId { get; set; }
    }
}
