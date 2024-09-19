using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCustomAssessmentEmailDetail
    {
        public long Id { get; set; }
        public long? AssessmentId { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailContent { get; set; }
    }
}
