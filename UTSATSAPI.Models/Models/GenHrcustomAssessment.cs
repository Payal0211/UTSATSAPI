using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenHrcustomAssessment
    {
        public long Id { get; set; }
        public long? HiringRequestId { get; set; }
        public int? Type { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public string? Skills { get; set; }
        public int? Duration { get; set; }
        public DateTime? SubmissionDeadline { get; set; }
        public string? CandidatesGuideline { get; set; }
        public string? AdditionalInfo { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? LastModifiedById { get; set; }
        public DateTime? LastModifiedByDateTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
