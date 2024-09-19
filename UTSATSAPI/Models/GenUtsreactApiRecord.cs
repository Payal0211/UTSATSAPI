using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenUtsreactApiRecord
    {
        public long Id { get; set; }
        public string? FromApiUrl { get; set; }
        public string? ToApiUrl { get; set; }
        public string? PayloadToSend { get; set; }
        public string? ResponseReceived { get; set; }
        public int? StatusCode { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedByDateTime { get; set; }
    }
}
