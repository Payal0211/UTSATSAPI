using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenUtsatsapiQueue
    {
        public long Id { get; set; }
        public long? Hrid { get; set; }
        public string? ControllerActionName { get; set; }
        public string? ToApiUrl { get; set; }
        public string? PayloadToSend { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public int? AppActionDoneBy { get; set; }
        public int? StatusId { get; set; }
        public int? ExecutionPriority { get; set; }
    }
}
