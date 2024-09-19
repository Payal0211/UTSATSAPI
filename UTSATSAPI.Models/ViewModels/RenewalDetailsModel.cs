using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class RenewalDetailsModel
    {
        public long OnBoardId { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public decimal? BillRate { get; set; }
        public decimal? PayRate { get; set; }
        public string? EngagementId { get; set; }
        public string? ContactName { get; set; }
        public string? Company { get; set; }
        public string? TalentName { get; set; }
        public decimal? NRPercentage { get; set; }
        public int? ContarctDuration { get; set; }
        public string? ReasonForBRPRChange { get; set; }
        public string? Currency { get; set; }
        public bool? IsContractOnGoing { get; set; }
    }
}
