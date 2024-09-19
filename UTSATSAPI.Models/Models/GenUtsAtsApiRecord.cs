using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenUtsAtsApiRecord
    {
        public long Id { get; set; }
        public string? FromApiUrl { get; set; }
        public string? ToApiUrl { get; set; }
        public string? PayloadToSend { get; set; }
        public string? ResponseReceived { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
        public long? HrId { get; set; }
    }
}
