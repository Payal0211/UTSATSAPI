using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyFundingDetail
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public string? Amount { get; set; }
        public string? FundingRound { get; set; }
        public string? Series { get; set; }
        public string? Month { get; set; }
        public string? Year { get; set; }
        public string? Investors { get; set; }
        public long? CreatedById { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public long? ModifiedById { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string? AdditionalInformation { get; set; }
    }
}
