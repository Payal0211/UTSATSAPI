using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_CreditWithAmount_Result
    {
        public int ID { get; set; }
        public decimal? CreditAmount { get; set; }
        public string? CurrenyCode { get; set; }
        public decimal? INRAmount { get; set; }
    }
}
