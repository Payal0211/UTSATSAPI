using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyCultureandPerksDetail
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? CultureImage { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
