using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTalentEducationDetail
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? InstituteName { get; set; }
        public string? Degree { get; set; }
        public string? Fieldofstudy { get; set; }
        public bool? IsCurrentlyWorking { get; set; }
        public int? PassingYear { get; set; }
    }
}
