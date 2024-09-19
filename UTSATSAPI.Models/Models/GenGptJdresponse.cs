using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenGptJdresponse
    {
        public long Id { get; set; }
        public string? Url { get; set; }
        public string? Jdtext { get; set; }
        public string? ResponseData { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? AchievedDetails { get; set; }
        public string? Gptprompt { get; set; }
    }
}
