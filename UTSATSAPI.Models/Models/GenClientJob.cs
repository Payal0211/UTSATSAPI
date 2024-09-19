using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenClientJob
    {
        public long Id { get; set; }
        public string? Guid { get; set; }
        public long? ContactId { get; set; }
        public int? CurrentStepId { get; set; }
        public int? NextStepId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastModifiedByDateTime { get; set; }
        public bool? IsActive { get; set; }
    }
}
