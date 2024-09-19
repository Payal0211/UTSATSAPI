using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ClaudeAIResponse
    {
        public List<string> Requirements { get; set; }
        public List<string> Roles_And_Responsibilities { get; set; }
        public int? Years_Of_Experience { get; set; }
        public decimal? Budget_From { get; set; }
        public decimal? Budget_To { get; set; }
        public decimal? Max_Salary { get; set; }
        public string Salary_Curreny { get; set; }
        public string Working_Zone_With_Time_Zone { get; set; }
        public string Type_Of_Job { get; set; }
        public string Opportunity_Type { get; set; }
        public string Skills { get; set; }
        public string Job_Title { get; set; }
        [JsonProperty("Suggested Skills")]
        public string Suggested_Skills { get; set; }
        public long? prompt_tokens { get; set; }
        public long? completion_tokens { get; set; }
        public long? total_tokens { get; set; }
    }
}
