using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAwsTrackingEmailSubject
    {
        public long Id { get; set; }
        public string? EmailSubject { get; set; }
        public string? DisplayName { get; set; }
    }
}
