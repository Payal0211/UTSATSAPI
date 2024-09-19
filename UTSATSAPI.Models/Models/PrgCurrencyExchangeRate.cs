using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class PrgCurrencyExchangeRate
    {
        public int Id { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencySign { get; set; }
        public decimal? ExchangeRate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string? CountryCode { get; set; }
        public string? Culture { get; set; }
    }
}
