using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class StoreApiurl
    {
        public long Id { get; set; }
        public long? Hrid { get; set; }
        public long? HistoryId { get; set; }
        public string? Apiurl { get; set; }
        public string? ResponseStatus { get; set; }
        public DateTime? CreatedDt { get; set; }
        public string? Payload { get; set; }
        public bool? FromAts { get; set; }
    }
}
