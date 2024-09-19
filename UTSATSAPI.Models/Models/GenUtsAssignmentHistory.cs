using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenUtsAssignmentHistory
    {
        public long Id { get; set; }
        public long? Geoid { get; set; }
        public string? EmailId { get; set; }
        public string? NbdLeadEmployeeId { get; set; }
        public string? SalesPersonEmployeeId { get; set; }
        public string JsonRequest { get; set; } = null!;
        public string JsonResponse { get; set; } = null!;
        public int CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public long? CompanyId { get; set; }
    }
}
