using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class RestoreChannel
    {
        public long Id { get; set; }
        public long? Hrid { get; set; }
        public string? UserEmpId { get; set; }
        public string? PerformAction { get; set; }
        public string? ResponseStatus { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string? ChannelId { get; set; }
    }
}
