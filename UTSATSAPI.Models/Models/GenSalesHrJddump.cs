using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenSalesHrJddump
    {
        public long Id { get; set; }
        public long? ContactId { get; set; }
        public string? Company { get; set; }
        public string? TimeZone { get; set; }
        public string? Jdrole { get; set; }
        public string? Jddescription { get; set; }
        public string? Jdskills { get; set; }
        public string? HowSoon { get; set; }
        public string? HowLong { get; set; }
        public string? YearsOfExp { get; set; }
        public string? Qty { get; set; }
        public string? Jdtools { get; set; }
        public string? JdfileName { get; set; }
        public string? Jddetails { get; set; }
        public DateTime? CreatedByDatetime { get; set; }
        public string? JdallSkills { get; set; }
        public string? RolesResponsibilities { get; set; }
        public string? Requirement { get; set; }
        public decimal? JdskillPercentage { get; set; }
        public decimal? JdRolesResponsibilities { get; set; }
        public decimal? JdRequirement { get; set; }
    }
}
