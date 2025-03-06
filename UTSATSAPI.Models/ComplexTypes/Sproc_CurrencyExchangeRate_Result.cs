using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_CurrencyExchangeRate_Result
    {
        public int? ID { get; set; }
        public string? CurrencyCode { get; set; }
        public string? CurrencySign { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? USD_ExchangeRate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? TotalRecords { get; set; }
    }
}
