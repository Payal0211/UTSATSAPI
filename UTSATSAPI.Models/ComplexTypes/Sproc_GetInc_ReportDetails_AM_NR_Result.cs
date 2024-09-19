using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public class Sproc_GetInc_ReportDetails_AM_NR_Result
    {
        public long ID { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }
        public string ClientName { get; set; }
        public string EngagemenId { get; set; }
        public string HR_Number { get; set; }
        public string TalentName { get; set; }
        public string CompanyCategory { get; set; }
        public string ClientClosureDate { get; set; }
        public int ContractPeriod { get; set; }
        public decimal NRValue { get; set; }
        public decimal BR { get; set; }
        public decimal PR { get; set; }
        public string NBDSalesPerson { get; set; }
        public string AMSalesPerson { get; set; }
        public string LeadType { get; set; }
        public decimal AM_NR_Percentage { get; set; }
        public string AM_NR_Slab { get; set; }
        public decimal Amount { get; set; }
        public string Company { get; set; }
        public string CB_Slab { get; set; }
        public decimal CB_SlabAmount { get; set; }
        public decimal CB_CalculatedAmount { get; set; }
        public string TI_Slab { get; set; }
        public decimal TI_SlabAmount { get; set; }
        public decimal TI_CalculatedAmount { get; set; }
        public string DP_Slab { get; set; }
        public decimal DP_SlabAmount { get; set; }
        public decimal DP_CalculatedAmount { get; set; }
    }
}
