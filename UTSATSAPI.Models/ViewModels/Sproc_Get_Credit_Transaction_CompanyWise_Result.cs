using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    [Keyless]
    public class Sproc_Get_Credit_Transaction_CompanyWise_Result
    {
        public long? HRID { get; set; }
        public string? HRNumber { get; set; }
        public string? TalentName { get; set; }
        public string? Company { get; set; }
        public string? Client { get; set; }
        public string? CreatedByDate { get; set; }
        public int? CreditBalance { get; set; }
        public string? PackageName { get; set; }
        public decimal? AmountPerCredit { get; set; }
        public string? CreditCurrency { get; set; }
        public decimal? CreditUsed { get; set; }

        public int? TotalRecords { get; set; }
        public decimal? VettedCount { get; set; }
        public decimal? NonVettedCount { get; set; }

        public decimal? JPCreditBalance { get; set; }
        public string? CurrentCurrency { get; set; }
        public decimal? CurrentAmount { get; set; }
        public string? ActionDescription { get; set; }
        public string? CreditSpent { get; set; }
        public string? CreditBalanceAfterSpent { get; set; }
        public string? CreditDebit { get; set; }
        public string? RequestTalent { get; set; }
    }
}
