using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenBlog
    {
        public long Id { get; set; }
        public string? Blog { get; set; }
        public string? BlogImage { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public string? BlogUrl { get; set; }
    }
}
