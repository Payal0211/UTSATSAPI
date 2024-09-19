using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_Get_HRDetails_From_DealId_Result
    {
        public string ClientEmail { get; set; }
        public string Company { get; set; }
        public string ClientName { get; set; }
        public string SalesUser { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Adhoc_BudgetCost { get; set; }
        public string Discovery_Call { get; set; }
        public Nullable<long> SalesUserID { get; set; }
        public long ClientID { get; set; }
        public long CompanyID { get; set; }
    }
}
