using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class sp_UTS_TalentCalculation_PayPerHire_Result
    {
        public decimal CalculatedClientPay { get; set; }
        public decimal CalculatedTalentPay { get; set; }
        public decimal CalculatedUplersFeesInAmount { get; set; }
        public decimal CalculatedUplersFeesInPer { get; set; }

    }
}
