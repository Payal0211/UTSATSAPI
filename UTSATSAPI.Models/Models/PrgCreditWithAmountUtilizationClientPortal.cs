using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgCreditWithAmountUtilizationClientPortal
    {
        public int Id { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CurrenyCode { get; set; }
        public decimal? Inramount { get; set; }
    }
}
