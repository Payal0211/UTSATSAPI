using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CompanyDetailsViewModel
    {
        [JsonProperty("Company URL")]
        public string? CompanyURL { get; set; }

        [JsonProperty("Company Size")]
        public int? CompanySize { get; set; }

        [JsonProperty("Company Industry Type")]
        public string? CompanyIndustryType { get; set; }

        [JsonProperty("Brief About Company")]
        public string? BriefAboutCompany { get; set; }
    }
}
