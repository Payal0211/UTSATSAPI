using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyAmMappingHistory
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public int OldAmSalesPersonId { get; set; }
        public int NewAmSalesPersonId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string? Comment { get; set; }
    }
}
