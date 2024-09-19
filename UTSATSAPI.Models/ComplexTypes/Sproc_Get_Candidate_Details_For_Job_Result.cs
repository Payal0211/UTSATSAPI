using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_Candidate_Details_For_Job_Result
    {
        public string? TalentName { get; set; }
        public string? Designation { get; set; }
        public string? AISummary { get; set; }
        public string? IsVideoResume { get; set; }
        public string? VideoVetting { get; set; }
        public string? TalentResume { get; set; }
    }
}
