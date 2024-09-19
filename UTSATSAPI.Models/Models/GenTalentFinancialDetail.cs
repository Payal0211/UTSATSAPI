using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenTalentFinancialDetail
    {
        public long Id { get; set; }
        public long TalentId { get; set; }
        public string LegalName { get; set; } = null!;
        public string ContactNumber { get; set; } = null!;
        public string EmailId { get; set; } = null!;
        public string Panno { get; set; } = null!;
        public string? Gstno { get; set; }
        public string Address1 { get; set; } = null!;
        public string PinCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string? AccountHolderName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? Ifsccode { get; set; }
        public string? AccountType { get; set; }
        public string? BranchBankName { get; set; }
        public string? NatureOfService { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedById { get; set; }
        public int? LastModifyById { get; set; }
        public DateTime? LastModifyDatetime { get; set; }
    }
}
