using System;
using System.Collections.Generic;

namespace UTSATSAPI.Models.Models
{
    public partial class GenCompanyLeadTypeUserDetail
    {
        public long Id { get; set; }
        public long? CompanyId { get; set; }
        public int? LeadTypeUserId { get; set; }
    }
}
