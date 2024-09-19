using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class SP_PayPerHire_CalculationDetails_Result
    {
        public bool? IsTransparentPricing { get; set; }
        public string? Budget {  get; set; }
        public string? UplersFeeInAmount { get; set; }
        public string? ClientPay { get; set; }
        public string? TalentPay { get; set; }
    }
}
