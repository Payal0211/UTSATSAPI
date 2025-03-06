using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class ContractualDpViewModel
    {
        public ContractualDpViewModel()
        {
            TalentDetails = new List<HRTalentDetail>();
        }
        public long HRID { get; set; }
        public string CreatedByDateTime { get; set; }
        public string CreatedByID { get; set; }
        public string HR_Currency { get; set; }

        public List<HRTalentDetail> TalentDetails { get; set; }
    }

    public class HRTalentDetail
    {
        public long CTPID { get; set; }
        public long UTS_TalentID { get; set; }
        public long ATS_TalentID { get; set; }
        public decimal? Talent_current_fee { get; set; }
        public decimal? Talent_Expected_fee { get; set; }
        public string HR_Type { get; set; }
        public decimal? DPorNR_Percent { get; set; }
        public decimal? ExchangeRateUTS { get; set; }
        public string TalentCurrency { get; set; }
        public decimal? DPAmount { get; set; }
        public string HR_Cost_With_Currency { get; set; }
        public decimal? HR_Cost { get; set; }
        public string HRPricingType { get; set; }
        public decimal? UplersFee { get; set; }
        public decimal? UplersFeeAmount { get; set; }
        public string YearsOfExperience { get; set; }
        public string NoticePeriod { get; set; }
        public string AgreedShift { get; set; }
        public string PreferredAvailability { get; set; }
        public string Talent_Expected_fee_yearly { get; set; }
        public string Talent_current_fee_yearly { get; set; }
        public string HRTypeText { get; set; }
        public string AISummary { get; set; }
        public string IsVideoResume { get; set; }
        public string VideoVetting { get; set; }
        public string TalentResume { get; set; }
    }
}
