using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ViewModels
{
    public class CurrencyExchangeRateViewModel
    {
        public int? ID { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencySign { get; set; }
        public decimal? ExchangeRate { get; set; }
        public string? LastUpdatedDate { get; set; }
    }
}
