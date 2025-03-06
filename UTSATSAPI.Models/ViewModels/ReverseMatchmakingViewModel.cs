using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ReverseMatchmakingViewModel
    {
        public long HRID { get; set; }
        public long ATS_TalentID { get; set; }
        public decimal? monthly_salary_in_usd { get; set; }        
        public DateTime CreatedByDateTime { get; set; }
        public string CreatedByID { get; set; }
        public string Talent_CurrencyCode { get; set; }
        public string ATS_Talent_LiveURL { get; set; }
        public string ATS_Non_NDAURL { get; set; }
        public string availibility { get; set; }
        public string shift { get; set; }
        public string notice { get; set; }
        public string resume { get; set; }
    }
}
