using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Add_Company_Transactions_With_ATS_Result
    {
        public long? companyid { get; set; }
        public decimal? credit_balance { get; set; }
        public int? free_credit_balance { get; set; }
        public int? paid_credit_balance { get; set; }
    }
}
