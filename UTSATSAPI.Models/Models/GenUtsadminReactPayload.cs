using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenUtsadminReactPayload
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public long? Hrid { get; set; }
        public string? Apiname { get; set; }
        public string? Payload { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public int? AppActionDoneBy { get; set; }
    }
}
