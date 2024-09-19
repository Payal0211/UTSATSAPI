using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSATSAPI.Models.ComplexTypes
{
    [Keyless]
    public partial class sproc_UTS_GetDealCompanyDetails_Result
    {
        public string URL { get; set; }
        public string Stage { get; set; }
        public string DealOwner { get; set; }
        public string DealType { get; set; }
        public string POC { get; set; }
        public int Size { get; set; }
        public string Address { get; set; }
        public string linkedin { get; set; }
        public string Phone { get; set; }
        public string LeadSOurce { get; set; }
        public string Location { get; set; }
        public string Company { get; set; }
        public string Companylogo { get; set; }
        public long CompanyId { get; set; }
    }
}
