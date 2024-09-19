using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenTestimonial
    {
        public long Id { get; set; }
        public string? Testimonials { get; set; }
        public string? GivenBy { get; set; }
        public string? Company { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
    }
}
