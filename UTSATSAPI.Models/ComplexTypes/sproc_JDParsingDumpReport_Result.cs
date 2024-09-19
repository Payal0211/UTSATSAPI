using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sproc_JDParsingDumpReport_Result
    {
        public string? HRCreatedDate { get; set; }
        public string? HRNumber { get; set; }
        public string? JDLink { get; set; }
        public string? JDFileName { get; set; }
        public string? JDDumpSkill { get; set; }
        public string? JDDumpRolesResponsibilities { get; set; }
        public string? JDDumpRequirement { get; set; }
        public string? HRRolesResponsibilities { get; set; }
        public string? HRRequirement { get; set; }
        public string? HRSkill { get; set; }
        public int? TotalRecords { get; set; }
        public decimal? JDSkillPercentage { get; set; }
        public decimal? JDRolesResponsibilities { get; set; }
        public decimal? JDRequirement { get; set; }
        public decimal? OverAllPercentage { get; set; }
        public decimal? OverAllRowWise { get; set; }
        public decimal? SkillPercentage { get; set; }
        public decimal? RolesResponsibilitiesPercentage { get; set; }
        public decimal? RequirementPercentage { get; set; }
    }
}
