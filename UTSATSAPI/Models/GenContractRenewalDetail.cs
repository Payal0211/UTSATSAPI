using System;
using System.Collections.Generic;

namespace TalentConnectAdminAPI.Models
{
    public partial class GenContractRenewalDetail
    {
        public long Id { get; set; }
        public long OnBoardId { get; set; }
        public DateTime RenewalStartDate { get; set; }
        public DateTime RenewalEndDate { get; set; }
        public decimal ContractDuration { get; set; }
        public decimal NewBr { get; set; }
        public decimal NewPr { get; set; }
        public decimal NewNr { get; set; }
        public string? Reason { get; set; }
        public long CreatedById { get; set; }
        public DateTime CreatedByDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedByDateTime { get; set; }
        public bool? IsContractRenewed { get; set; }
    }
}
