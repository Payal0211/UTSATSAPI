using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenJobPostVitalInfoClientPortal
    {
        public long Id { get; set; }
        public string? Guid { get; set; }
        public long? ContactId { get; set; }
        public string? CompensationOption { get; set; }
        public string? IndustryType { get; set; }
        public bool? HasPeopleManagementExp { get; set; }
        public string? Prerequisites { get; set; }
        public string? StringSeparator { get; set; }
        public long? CreatedbyId { get; set; }
        public DateTime? CreatedbyDateTime { get; set; }
        public long? ModifiedbyId { get; set; }
        public DateTime? ModifiedbyDateTime { get; set; }
        public bool? IsActive { get; set; }
        public long? Hrid { get; set; }
    }
}
