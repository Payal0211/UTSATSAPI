using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class GetJDParsingDumpReportViewModel : CommonFilterModel
    {
        public string? HRNumber { get; set; }
        public decimal? JDSkillPercentage { get; set; }
        public decimal? JDRolesResponsibilities { get; set; }
        public decimal? JDRequirement { get; set; }
        public decimal? OverAllRowWise { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
