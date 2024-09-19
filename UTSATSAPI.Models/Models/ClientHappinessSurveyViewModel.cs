using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.Models
{
    public class ClientHappinessSurveyViewModel
    {
        public int? ID { get; set; }
        public string? Company { get; set; }
        public int? Company_ID { get; set; }
        public IEnumerable<SelectListItem> Client { get; set; }
        public int? Client_ID { get; set; }
        public string? Client_Name { get; set; }
        public string ?Email { get; set; }
        public string? Link { get; set; }
        public string? Other_Client_Name { get; set; }
        public string? Other_ClientEmail { get; set; }
        public string? Other_Company_Name { get; set; }
        public List<SelectListItem> OptionData { get; set; }

        public List<string> OptionValue { get; set; }
    }

    public class HappinessSurveyFeedback
    {
        public string? Name { get; set;}
        public string? ProjectUrl { get; set; }
        public string? Email { get; set; }
        public IEnumerable<PrgClientHappinessSurveyFeedbackOption>? Option { get; set; }
        public string? HappinessSurvey_Question { get; set; }
        public int? QuestionID { get; set; }
        public string? Comments { get; set; }
        public long? HappinessSurveyID { get; set;}
        public int? Ratings { get; set; }

    }

}
