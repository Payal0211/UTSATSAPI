using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenAssessmentWiseTalentDetail
    {
        public long Id { get; set; }
        public long? AssessmentId { get; set; }
        public long? AtstalentId { get; set; }
        public bool? IsSharedToTalent { get; set; }
    }
}
