using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyDetail
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? Culture { get; set; }
        public string? FoundedYear { get; set; }
        public string? CompanyType { get; set; }
        public string? Headquaters { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public bool? IsSelfFunded { get; set; }
        public string? TotalFundings { get; set; }
    }
}
