using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentTestimonialOrFeedback
    {
        public long Id { get; set; }
        public long? TalentId { get; set; }
        public string? TestimonialOrFeedback { get; set; }
        public string? ClientName { get; set; }
        public string? Role { get; set; }
        public string? SkillIds { get; set; }
    }
}
