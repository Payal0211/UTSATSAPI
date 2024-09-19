using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyYouTubeLink
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? YoutubeLink { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }
}
