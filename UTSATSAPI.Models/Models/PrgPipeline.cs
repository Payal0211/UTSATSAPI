using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgPipeline
    {
        public int Id { get; set; }
        public string? PipelineId { get; set; }
        public string? PipelineLabel { get; set; }
    }
}
